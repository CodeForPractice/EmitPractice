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

        public static DynamicMethod GetDynamicMethod<T>(this DataRow @this)
        {
            var instanceType = typeof(T);
            var isContainColumnMethod = typeof(DataTableUtil).GetMethod("IsContainColumn", new Type[] { typeof(DataRow), typeof(string) });
            var getColumnValueMethod = typeof(DataTableUtil).GetMethod("GetColumnValue", new Type[] { typeof(DataRow), typeof(string) });
            DynamicMethod convertMethod = new DynamicMethod("ConvertMethod" + instanceType.Name, instanceType, new Type[] { typeof(DataRow) }, true);

            ILGenerator il = convertMethod.GetILGenerator();
            LocalBuilder instance = il.DeclareLocal(instanceType);
            il.Emit(OpCodes.Newobj, instanceType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, instance);

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
                    il.Emit(OpCodes.Unbox_Any, item.PropertyType);//如果是值类型就拆箱
                }
                else
                {
                    il.Emit(OpCodes.Castclass, item.PropertyType);
                }
                //il.Emit(OpCodes.Unbox_Any, item.PropertyType);//如果是值类型就拆箱
                il.Emit(OpCodes.Callvirt, item.GetSetMethod());

                il.MarkLabel(endIfLabel);
            }

            foreach (var item in properties.Where(m => !(TypeUtil._BaseTypes.Contains(m.PropertyType) || m.PropertyType.IsEnum)))
            {
                if (item.PropertyType.IsArray || item.PropertyType.IsGenericType)
                {

                }else
                {

                }
            }
            il.Emit(OpCodes.Ldloc, instance);
            il.Emit(OpCodes.Ret);
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
    }
}
