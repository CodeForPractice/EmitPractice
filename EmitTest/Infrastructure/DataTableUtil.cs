using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：DataTableUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/6/2 16:50:12
    /// </summary>
    public static class DataTableUtil
    {
        #region 将DataRow转换成对应的实体。

        /// <summary>
        ///  将DataRow转换成对应的实体。
        ///  创建标识：yjq 2016年7月21日15:07:01
        /// </summary>
        /// <typeparam name="T">要转换的实体类型。</typeparam>
        /// <param name="this">被转换的DataRow</param>
        /// <returns>转换的实体结果</returns>
        public static T ToEntity<T>(this DataRow @this) where T : new()
        {
            Type type = typeof(T);
            List<PropertyInfo> properties = PropertyUtil.GetTypeProperties(type);
            FieldInfo[] fields = TypeUtil.GetTypeFields(type);

            var entity = new T();

            foreach (PropertyInfo property in properties)
            {
                if (@this.Table.Columns.Contains(property.Name))
                {
                    Type valueType = property.PropertyType;
                    property.SetValue(entity, @this[property.Name].To(valueType), null);
                }
            }

            foreach (FieldInfo field in fields)
            {
                if (@this.Table.Columns.Contains(field.Name))
                {
                    Type valueType = field.FieldType;
                    field.SetValue(entity, @this[field.Name].To(valueType));
                }
            }

            return entity;
        }

        #endregion 将DataRow转换成对应的实体。

        public static object To(this Object @this, Type type)
        {
            if (@this != null)
            {
                Type targetType = type;

                if (@this.GetType() == targetType)
                {
                    return @this;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(@this);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return converter.ConvertTo(@this, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        return converter.ConvertFrom(@this);
                    }
                }

                if (@this == DBNull.Value)
                {
                    return null;
                }
            }

            return @this;
        }

        /// <summary>
        ///     Enumerates to entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an IEnumerable&lt;T&gt;</returns>
        public static IEnumerable<T> ToEntities<T>(this DataTable @this) where T : new()
        {
            Type type = typeof(T);
            List<PropertyInfo> properties = PropertyUtil.GetTypeProperties(type);
            FieldInfo[] fields = TypeUtil.GetTypeFields(type);

            var list = new List<T>();

            foreach (DataRow dr in @this.Rows)
            {
                var entity = new T();

                foreach (PropertyInfo property in properties)
                {
                    if (@this.Columns.Contains(property.Name))
                    {
                        Type valueType = property.PropertyType;
                        property.SetValue(entity, dr[property.Name].To(valueType), null);
                    }
                }

                foreach (FieldInfo field in fields)
                {
                    if (@this.Columns.Contains(field.Name))
                    {
                        Type valueType = field.FieldType;
                        field.SetValue(entity, dr[field.Name].To(valueType));
                    }
                }
                list.Add(entity);
            }

            return list;
        }

        public delegate T LoadDataRecord<T>(DataRow dr);
        public delegate List<T> LoadData<T>(DataTable ta);
        private static ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod> dynamicMethodCache = new ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod>();

        public static T ToT<T>(DataRow dr)
        {
            var type = typeof(T);
            if (type == null) return default(T);
            var typeHandle = type.TypeHandle;
            var method = dynamicMethodCache.GetValue(typeHandle, () =>
            {
                return GetDynamicMethod<T>(dr);
            });
            var deledge = (LoadDataRecord<T>)method.CreateDelegate(typeof(LoadDataRecord<T>));
            return deledge(dr);
        }

        private static readonly MethodInfo isContainColumnMethod = typeof(DataTableUtil).GetMethod("IsContainColumn", new Type[] { typeof(DataRow), typeof(string) });
        private static readonly MethodInfo getColumnValueMethod = typeof(DataTableUtil).GetMethod("GetColumnValue", new Type[] { typeof(DataRow), typeof(string) });
        private static readonly MethodInfo typeConvertMethod = typeof(DataTableUtil).GetMethod("To", new Type[] { typeof(object), typeof(Type) });
        private static readonly MethodInfo getRowMethod = typeof(DataTable).GetMethod("get_Rows", Type.EmptyTypes);
        private static readonly MethodInfo getRowItemMethod = typeof(DataRowCollection).GetMethod("get_Item", new Type[] { typeof(int) });

        public static void SetILGenerator(Type instanceType, ILGenerator il)
        {
            LocalBuilder instance = il.DeclareLocal(instanceType);
            il.Emit(OpCodes.Newobj, instanceType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, instance);
            SetInstancePropertyIL(instanceType, il, instance);
            il.Emit(OpCodes.Ldloc, instance);
            il.Emit(OpCodes.Ret);
        }

        private static void SetInstancePropertyIL(Type instanceType, ILGenerator il, LocalBuilder instance)
        {
            List<PropertyInfo> properties = PropertyUtil.GetTypeProperties(instanceType);

            foreach (var item in properties.Where(m => TypeUtil._BaseTypes.Contains(m.PropertyType) || m.PropertyType.IsEnum))
            {
                Label endIfLabel = il.DefineLabel();

                //判断DataRow是否包含该属性
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, item.Name);
                il.Emit(OpCodes.Call, isContainColumnMethod);

                il.Emit(OpCodes.Brfalse, endIfLabel);

                //获取该属性在datarow的值
                il.Emit(OpCodes.Ldloc, instance);

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, item.Name);
                il.Emit(OpCodes.Call, getColumnValueMethod);

                //设置值
                if (item.PropertyType.IsValueType)
                {
                    var convertMethod = MethodUtil.GetMethodInfo(item.PropertyType);
                    if (convertMethod == null)
                    {
                        il.Emit(OpCodes.Unbox_Any, item.PropertyType);//如果是值类型就拆箱
                    }
                    else
                    {
                        il.Emit(OpCodes.Call, convertMethod);
                    }
                }
                else
                {
                    il.Emit(OpCodes.Castclass, item.PropertyType);
                }
                il.Emit(OpCodes.Callvirt, item.GetSetMethod(true));

                il.MarkLabel(endIfLabel);
            }
            foreach (var item in properties.Where(m => !(TypeUtil._BaseTypes.Contains(m.PropertyType) || m.PropertyType.IsEnum)))
            {
                if (item.PropertyType.IsArray || item.PropertyType.IsGenericType)
                {

                }
                else
                {
                    LocalBuilder instanceProperty = il.DeclareLocal(item.PropertyType);
                    il.Emit(OpCodes.Newobj, item.PropertyType.GetConstructor(Type.EmptyTypes));
                    il.Emit(OpCodes.Stloc, instanceProperty);
                    SetInstancePropertyIL(item.PropertyType, il, instanceProperty);

                    il.Emit(OpCodes.Ldloc, instance);
                    il.Emit(OpCodes.Ldloc, instanceProperty);
                    il.Emit(OpCodes.Castclass, item.PropertyType);
                    il.Emit(OpCodes.Callvirt, item.GetSetMethod(true));
                }
            }

        }

        public static DynamicMethod GetDynamicMethod<T>(this DataRow @this)
        {
            var instanceType = typeof(T);
            DynamicMethod convertMethod = new DynamicMethod("ConvertMethod" + instanceType.Name, instanceType, new Type[] { typeof(DataRow) }, true);

            ILGenerator il = convertMethod.GetILGenerator();
            SetILGenerator(instanceType, il);
            return convertMethod;
        }



        public static bool IsContainColumn(DataRow row, string columnName)
        {
            if (row == null) return false;
            if (string.IsNullOrWhiteSpace(columnName)) return false;
            return row.Table.Columns.IndexOf(columnName) >= 0;
        }

        public static object GetColumnValue(DataRow row, string columnName)
        {
            if (IsContainColumn(row, columnName))
            {
                var value = row[columnName];
                if (value == null || value == DBNull.Value)
                {
                    value = null;
                }
                return value;
            }
            else
            {
                return null;
            }
        }

        public static void ToList<T>(DataRow row)
        {
            AssemblyUtil.GetTypeBuilder("Student", (typeBuilder, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("ToList", MethodAttributes.Public, typeof(T), new Type[] { typeof(DataRow) });

                ILGenerator il = methodBuilder.GetILGenerator();

                SetILGenerator(typeof(T), il);
                Type type = typeBuilder.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);

                object obj = Activator.CreateInstance(type);
                type.GetMethod("ToList").Invoke(obj, new object[] { row });

            });
        }

        public static Action<object, object[]> CreatePropertiesAction(PropertyInfo[] infos)
        {
            Type classType = GetClassTypeByProperty(infos);
            DynamicMethod method = new DynamicMethod("", null, new Type[] { typeof(object), typeof(object[]) }, true);
            ILGenerator il = method.GetILGenerator();

            LocalBuilder obj = il.DeclareLocal(classType);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Unbox_Any, classType); //对要赋值的对象进行拆箱
            il.Emit(OpCodes.Stloc_0);

            for (int i = 0; i < infos.Length; i++)
            {
                Label lbl_end = il.DefineLabel();
                Type propType = infos[i].PropertyType;

                il.Emit(OpCodes.Ldarg_1);
                Ldc(il, i);
                il.Emit(OpCodes.Ldelem_Ref); //定位i处的value

                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ceq);
                il.Emit(OpCodes.Brtrue_S, lbl_end); //判断是否为null，为null则跳过

                il.Emit(OpCodes.Ldloc_0); //对象压栈
                il.Emit(OpCodes.Ldarg_1); //值数组压栈
                Ldc(il, i);               //压入索引
                il.Emit(OpCodes.Ldelem_Ref); //取索引处的值
                il.Emit(OpCodes.Unbox_Any, propType); //拆箱

                il.Emit(OpCodes.Callvirt, infos[i].GetSetMethod()); //调用属性的set方法给属性赋值
                il.MarkLabel(lbl_end);
            }

            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Action<object, object[]>)) as Action<object, object[]>;
        }

        public static Func<object, object[]> CreatePropertiesFunc(PropertyInfo[] infos)
        {
            Type classType = GetClassTypeByProperty(infos);
            DynamicMethod method = new DynamicMethod("", typeof(object[]), new Type[] { typeof(object) }, true);
            ILGenerator il = method.GetILGenerator();

            LocalBuilder tmp = il.DeclareLocal(typeof(object));
            LocalBuilder result = il.DeclareLocal(typeof(object[]));

            LocalBuilder obj = il.DeclareLocal(classType);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Unbox_Any, classType);
            il.Emit(OpCodes.Stloc, obj);

            Ldc(il, infos.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            il.Emit(OpCodes.Stloc, result); //初始化一个object数组

            for (int i = 0; i < infos.Length; i++)
            {
                il.Emit(OpCodes.Ldloc, obj);
                il.Emit(OpCodes.Callvirt, infos[i].GetGetMethod()); //获取属性的值

                if (infos[i].PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, infos[i].PropertyType); //值类型则装箱

                il.Emit(OpCodes.Stloc, tmp); //保存到临时变量

                il.Emit(OpCodes.Ldloc, result);
                Ldc(il, i);
                il.Emit(OpCodes.Ldloc, tmp); //数组对象、索引位置、值分别压栈
                il.Emit(OpCodes.Stelem_Ref); //赋值
            }

            il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);

            return method.CreateDelegate(typeof(Func<object, object[]>)) as Func<object, object[]>;
        }

        private static Type GetClassTypeByProperty(PropertyInfo[] infos)
        {
            if (infos == null || infos.Length <= 0)
                throw new ArgumentNullException("infos");

            return infos[0].ReflectedType;
        }

        private static void Ldc(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
            else
                il.Emit(OpCodes.Ldc_I4, value);
        }


        private static DynamicMethod CreateToListMethod<T>(DataTable table)
        {
            var listType = typeof(List<T>);
            var instanceType = typeof(T);
            DynamicMethod convertMethod = new DynamicMethod("ConvertListMethod" + instanceType.Name, listType, new Type[] { typeof(DataTable) });

            ILGenerator il = convertMethod.GetILGenerator();

            LocalBuilder listBuilder = il.DeclareLocal(listType);//List<T> 存储对象

            LocalBuilder currentRowBuilder = il.DeclareLocal(typeof(DataRow));// T 存储对象
            LocalBuilder totalCountBuilder = il.DeclareLocal(typeof(int));// table 总记录数
            LocalBuilder currentIndexBuilder = il.DeclareLocal(typeof(int));// 当前table.Rows的索引

            Label exit = il.DefineLabel();//退出循环
            Label loop = il.DefineLabel();//循环体

            il.Emit(OpCodes.Newobj, listType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc_S, listBuilder);

            il.Emit(OpCodes.Ldc_I4, table.Rows.Count);
            il.Emit(OpCodes.Stloc_S, totalCountBuilder);

            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_S, currentIndexBuilder);

            il.MarkLabel(loop);//开始循环
            LocalBuilder instanceBuilder = il.DeclareLocal(instanceType);// T 存储对象
            il.Emit(OpCodes.Newobj, instanceType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc_S, instanceBuilder);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, getRowMethod);
            il.Emit(OpCodes.Ldloc, currentIndexBuilder);
            il.Emit(OpCodes.Callvirt, getRowItemMethod);//table.Rows[i]
            il.Emit(OpCodes.Stloc_S, currentRowBuilder);

            SetDataRowIL(instanceType, il, currentRowBuilder, instanceBuilder);

            il.Emit(OpCodes.Ldloc, listBuilder);
            il.Emit(OpCodes.Ldloc, instanceBuilder);
            il.Emit(OpCodes.Call, listType.GetMethod("Add"));

            //i++
            il.Emit(OpCodes.Ldloc, currentIndexBuilder);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_S, currentIndexBuilder);
            //i<table.Rows.Count
            il.Emit(OpCodes.Ldloc, currentIndexBuilder);
            il.Emit(OpCodes.Ldloc, totalCountBuilder);
            il.Emit(OpCodes.Clt);
            il.Emit(OpCodes.Brtrue, loop);

            il.Emit(OpCodes.Ldloc, listBuilder);
            il.Emit(OpCodes.Ret);

            return convertMethod;
        }
        private static ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod> _ConvertListMethodCache = new ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod>();
        public static List<T> ToList<T>(DataTable table)
        {
            if (table == null || table.Rows.Count <= 0)
            {
                return new List<T>();
            }

            var convertMethod = _ConvertListMethodCache.GetValue(typeof(T).TypeHandle, () =>
            {
                return CreateToListMethod<T>(table);
            });

            //var listType = typeof(List<T>);
            //var instanceType = typeof(T);
            //DynamicMethod convertMethod = new DynamicMethod("ConvertListMethod" + instanceType.Name, listType, new Type[] { typeof(DataTable) });

            //ILGenerator il = convertMethod.GetILGenerator();

            //LocalBuilder listBuilder = il.DeclareLocal(listType);//List<T> 存储对象

            //LocalBuilder currentRowBuilder = il.DeclareLocal(typeof(DataRow));// T 存储对象
            //LocalBuilder totalCountBuilder = il.DeclareLocal(typeof(int));// table 总记录数
            //LocalBuilder currentIndexBuilder = il.DeclareLocal(typeof(int));// 当前table.Rows的索引

            //Label exit = il.DefineLabel();//退出循环
            //Label loop = il.DefineLabel();//循环体

            //il.Emit(OpCodes.Newobj, listType.GetConstructor(Type.EmptyTypes));
            //il.Emit(OpCodes.Stloc_S, listBuilder);

            //il.Emit(OpCodes.Ldc_I4, table.Rows.Count);
            //il.Emit(OpCodes.Stloc_S, totalCountBuilder);

            //il.Emit(OpCodes.Ldc_I4_0);
            //il.Emit(OpCodes.Stloc_S, currentIndexBuilder);

            //il.MarkLabel(loop);//开始循环
            //LocalBuilder instanceBuilder = il.DeclareLocal(instanceType);// T 存储对象
            //il.Emit(OpCodes.Newobj, instanceType.GetConstructor(Type.EmptyTypes));
            //il.Emit(OpCodes.Stloc_S, instanceBuilder);
            //il.Emit(OpCodes.Ldarg_0);
            //il.Emit(OpCodes.Callvirt, getRowMethod);
            //il.Emit(OpCodes.Ldloc, currentIndexBuilder);
            //il.Emit(OpCodes.Callvirt, getRowItemMethod);//table.Rows[i]
            //il.Emit(OpCodes.Stloc_S, currentRowBuilder);

            //SetDataRowIL(instanceType, il, currentRowBuilder, instanceBuilder);

            //il.Emit(OpCodes.Ldloc, listBuilder);
            //il.Emit(OpCodes.Ldloc, instanceBuilder);
            //il.Emit(OpCodes.Call, listType.GetMethod("Add"));

            ////i++
            //il.Emit(OpCodes.Ldloc, currentIndexBuilder);
            //il.Emit(OpCodes.Ldc_I4_1);
            //il.Emit(OpCodes.Add);
            //il.Emit(OpCodes.Stloc_S, currentIndexBuilder);
            ////i<table.Rows.Count
            //il.Emit(OpCodes.Ldloc, currentIndexBuilder);
            //il.Emit(OpCodes.Ldloc, totalCountBuilder);
            //il.Emit(OpCodes.Clt);
            //il.Emit(OpCodes.Brtrue, loop);

            //il.Emit(OpCodes.Ldloc, listBuilder);
            //il.Emit(OpCodes.Ret);



            var deledge = (LoadData<T>)convertMethod.CreateDelegate(typeof(LoadData<T>));
             return deledge(table);

            //return null;

        }

        private static void SetDataRowIL(Type instanceType, ILGenerator il, LocalBuilder currentRowBuilder, LocalBuilder instanceBuilder)
        {
            List<PropertyInfo> properties = PropertyUtil.GetTypeProperties(instanceType);

            foreach (var item in properties.Where(m => TypeUtil._BaseTypes.Contains(m.PropertyType) || m.PropertyType.IsEnum))
            {
                Label endIfLabel = il.DefineLabel();

                //判断DataRow是否包含该属性

                il.Emit(OpCodes.Ldloc, currentRowBuilder);
                il.Emit(OpCodes.Ldstr, item.Name);
                il.Emit(OpCodes.Call, isContainColumnMethod);
                il.Emit(OpCodes.Brfalse, endIfLabel);

                //获取该属性在datarow的值
                il.Emit(OpCodes.Ldloc, instanceBuilder);

                il.Emit(OpCodes.Ldloc, currentRowBuilder);
                il.Emit(OpCodes.Ldstr, item.Name);
                il.Emit(OpCodes.Call, getColumnValueMethod);

                //设置值
                if (item.PropertyType.IsValueType)
                {
                    var convertMethod = MethodUtil.GetMethodInfo(item.PropertyType);
                    if (convertMethod == null)
                    {
                        il.Emit(OpCodes.Unbox_Any, item.PropertyType);//如果是值类型就拆箱
                    }
                    else
                    {
                        il.Emit(OpCodes.Call, convertMethod);
                    }
                }
                else
                {
                    il.Emit(OpCodes.Castclass, item.PropertyType);
                }
                il.Emit(OpCodes.Callvirt, item.GetSetMethod(true));

                il.MarkLabel(endIfLabel);
            }

            foreach (var item in properties.Where(m => !(TypeUtil._BaseTypes.Contains(m.PropertyType) || m.PropertyType.IsEnum)))
            {
                if (item.PropertyType.IsArray || item.PropertyType.IsGenericType)
                {

                }
                else
                {
                    LocalBuilder instanceProperty = il.DeclareLocal(item.PropertyType);
                    il.Emit(OpCodes.Newobj, item.PropertyType.GetConstructor(Type.EmptyTypes));
                    il.Emit(OpCodes.Stloc, instanceProperty);
                    SetDataRowIL(item.PropertyType, il, currentRowBuilder, instanceProperty);

                    il.Emit(OpCodes.Ldloc, instanceBuilder);
                    il.Emit(OpCodes.Ldloc, instanceProperty);
                    il.Emit(OpCodes.Castclass, item.PropertyType);
                    il.Emit(OpCodes.Callvirt, item.GetSetMethod(true));
                }
            }
        }

        public static void ILTest(DataTable table)
        {
            int rows = table.Rows.Count;
            int index = 1;
            var row = table.Rows[index];
        }
    }
}
