using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：ThirdyParty1.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/6/3 15:12:26
    /// </summary>
    /// <summary>
    /// 类属性成员与字段对应的属性定义
    /// </summary>
    public class FieldAttribute : Attribute
    {
        private string m_Field;

        /// <summary>
        /// 与数据关联的字段名称
        /// </summary>
        public string Field
        {
            get { return m_Field; }
            set { m_Field = value; }
        }

        /// <summary>
        /// 构造一个字段对应关系
        /// </summary>
        /// <param name="fieldName"></param>
        public FieldAttribute(string fieldName)
        {
            this.m_Field = fieldName;
        }
    }

    /// <summary>
    /// 数据库类型的值到本地类型的转换类
    /// </summary>
    internal static class DBConvert
    {
        public static bool IsDBNull(object value)
        {
            return object.Equals(DBNull.Value, value);
        }

        public static short ToInt16(object value)
        {
            if (value is short)
            {
                return (short)value;
            }
            try
            {
                return Convert.ToInt16(value);
            }
            catch
            {
                return 0;
            }
        }

        public static int ToInt32(object value)
        {
            if (value is int)
            {
                return (int)value;
            }
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        public static long ToInt64(object value)
        {
            if (value is long)
            {
                return (long)value;
            }
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return 0;
            }
        }

        public static bool ToBoolean(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is bool)
            {
                return (bool)value;
            }
            if (value.Equals("1") || value.Equals("-1"))
            {
                value = "true";
            }
            else if (value.Equals("0"))
            {
                value = "false";
            }

            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        public static DateTime ToDateTime(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static decimal ToDecimal(object value)
        {
            if (value is decimal)
            {
                return (decimal)value;
            }
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }

        public static double ToDouble(object value)
        {
            if (value is double)
            {
                return (double)value;
            }
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }

        public static Nullable<short> ToNInt16(object value)
        {
            if (value is short)
            {
                return new Nullable<short>((short)value);
            }
            try
            {
                return new Nullable<short>(Convert.ToInt16(value));
            }
            catch
            {
                return new Nullable<short>();
            }
        }

        public static Nullable<int> ToNInt32(object value)
        {
            if (value is int)
            {
                return new Nullable<int>((int)value);
            }
            try
            {
                return new Nullable<int>(Convert.ToInt32(value));
            }
            catch
            {
                return new Nullable<int>();
            }
        }

        public static Nullable<long> ToNInt64(object value)
        {
            if (value is long)
            {
                return new Nullable<long>((long)value);
            }
            try
            {
                return new Nullable<long>(Convert.ToInt64(value));
            }
            catch
            {
                return new Nullable<long>();
            }
        }

        public static Nullable<bool> ToNBoolean(object value)
        {
            if (value is bool)
            {
                return new Nullable<bool>((bool)value);
            }
            try
            {
                return new Nullable<bool>(Convert.ToBoolean(value));
            }
            catch
            {
                return new Nullable<bool>();
            }
        }

        public static Nullable<DateTime> ToNDateTime(object value)
        {
            if (value is DateTime)
            {
                return new Nullable<DateTime>((DateTime)value);
            }
            try
            {
                return new Nullable<DateTime>(Convert.ToDateTime(value));
            }
            catch
            {
                return new Nullable<DateTime>();
            }
        }

        public static Nullable<decimal> ToNDecimal(object value)
        {
            if (value is decimal)
            {
                return new Nullable<decimal>((decimal)value);
            }
            try
            {
                return new Nullable<decimal>(Convert.ToDecimal(value));
            }
            catch
            {
                return new Nullable<decimal>();
            }
        }

        public static Nullable<double> ToNDouble(object value)
        {
            if (value is double)
            {
                return new Nullable<double>((double)value);
            }
            try
            {
                return new Nullable<double>(Convert.ToDouble(value));
            }
            catch
            {
                return new Nullable<double>();
            }
        }
    }

    internal class Mapper
    {
        private static readonly MethodInfo Object_ToString = typeof(object).GetMethod("ToString");
        private static readonly MethodInfo Reader_Read = typeof(IDataReader).GetMethod("Read");
        private static readonly MethodInfo Reader_GetValues = typeof(IDataRecord).GetMethod("GetValues", new Type[] { typeof(object[]) });
        private static readonly MethodInfo Convert_IsDBNull = typeof(DBConvert).GetMethod("IsDBNull", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToInt16 = typeof(DBConvert).GetMethod("ToInt16", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToInt32 = typeof(DBConvert).GetMethod("ToInt32", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToInt64 = typeof(DBConvert).GetMethod("ToInt64", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToBoolean = typeof(DBConvert).GetMethod("ToBoolean", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToDateTime = typeof(DBConvert).GetMethod("ToDateTime", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToDecimal = typeof(DBConvert).GetMethod("ToDecimal", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToDouble = typeof(DBConvert).GetMethod("ToDouble", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToNullInt16 = typeof(DBConvert).GetMethod("ToNInt16", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToNullInt32 = typeof(DBConvert).GetMethod("ToNInt32", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToNullInt64 = typeof(DBConvert).GetMethod("ToNInt64", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToNullBoolean = typeof(DBConvert).GetMethod("ToNBoolean", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToNullDateTime = typeof(DBConvert).GetMethod("ToNDateTime", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToNullDecimal = typeof(DBConvert).GetMethod("ToNDecimal", new Type[] { typeof(object) });
        private static readonly MethodInfo Convert_ToNullDouble = typeof(DBConvert).GetMethod("ToNDouble", new Type[] { typeof(object) });

        private delegate T ReadEntityInvoker<T>(IDataReader dr);

        private static Dictionary<string, DynamicMethod> m_CatchMethod;

        /// <summary>
        /// 对类型T编写一个动态方法来设置其属性，并返回对动态方法的调用结果。
        /// 对于值是DBNull.Value的数据将不对属性进行赋值并保持对象的默认值。
        /// reader中的Select语句的字段顺序个数需保持一致。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> Map<T>(IDataReader reader)
        {
            if (reader == null || reader.IsClosed)
            {
                throw new Exception("连接已关闭！");
            }
            if (m_CatchMethod == null)
            {
                m_CatchMethod = new Dictionary<string, DynamicMethod>();
            }

            Type itemType = typeof(T);
            var key = itemType.FullName;
            if (!m_CatchMethod.ContainsKey(key))
            {
#if WRITE_FILE
                AssemblyName aName = new AssemblyName("DynamicAssembly");
                AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
                ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
                TypeBuilder tb = mb.DefineType("DynamicType", TypeAttributes.Public);
#endif
                Type listType = typeof(List<T>);
                Type objectType = typeof(object);
                Type objArrayType = typeof(object[]);
                Type boolType = typeof(bool);
                Type[] methodArgs = { typeof(IDataReader) };
                MethodInfo LAdd = listType.GetMethod("Add");
                PropertyInfo[] properties = null;
                getMapped(itemType, reader, out properties);
#if WRITE_FILE
                MethodBuilder dm = tb.DefineMethod("ReadEntities", MethodAttributes.Public| MethodAttributes.Static, listType, methodArgs);
#else
                DynamicMethod dm = new DynamicMethod("ReadEntities", listType, methodArgs, typeof(Mapper));
#endif
                //开始编写动态方法，动态方法在性能上优于反射，无需编译直接运行。
                ILGenerator ilg = dm.GetILGenerator();
                //List<T> list;
                LocalBuilder list = ilg.DeclareLocal(listType);
                //T item;
                LocalBuilder item = ilg.DeclareLocal(itemType);
                //object[] values;
                LocalBuilder values = ilg.DeclareLocal(objArrayType);
                //object objValue;
                LocalBuilder objValue = ilg.DeclareLocal(objectType);
                //type nulls
                LocalBuilder[] typeNulls = null;
                //设置类型对应的空值，以便在遇到DBNull.Value时用其值设置到对像成员上。
                initNulls(properties, ilg, out typeNulls);

                Label exit = ilg.DefineLabel();
                Label loop = ilg.DefineLabel();
                Label[] lblArray = new Label[properties.Length * 2];
                for (int i = 0; i < lblArray.Length; i++)
                {
                    lblArray[i] = ilg.DefineLabel();
                }
                //list = new List<T>();
                ilg.Emit(OpCodes.Newobj, listType.GetConstructor(Type.EmptyTypes));
                ilg.Emit(OpCodes.Stloc_S, list);

                //values=new object[FieldCount];
                ilg.Emit(OpCodes.Ldc_I4, reader.FieldCount);
                ilg.Emit(OpCodes.Newarr, objectType);
                ilg.Emit(OpCodes.Stloc_S, values);

                // while (arg.Read()) {
                ilg.MarkLabel(loop);
                ilg.Emit(OpCodes.Ldarg_0);
                ilg.Emit(OpCodes.Callvirt, Reader_Read);
                ilg.Emit(OpCodes.Brfalse, exit);

                //reader.GetValues(values);
                ilg.Emit(OpCodes.Ldarg_0);
                ilg.Emit(OpCodes.Ldloc_S, values);
                ilg.Emit(OpCodes.Callvirt, Reader_GetValues);
                ilg.Emit(OpCodes.Pop);

                //item=new T();
                ilg.Emit(OpCodes.Newobj, itemType.GetConstructor(Type.EmptyTypes));
                ilg.Emit(OpCodes.Stloc_S, item);

                //item.Property=Convert(values[index]);
                for (int index = 0; index < properties.Length; index++)
                {
                    PropertyInfo pi = properties[index];
                    if (pi == null)
                    {
                        continue;
                    }

                    //objValue=value[index];
                    ilg.Emit(OpCodes.Ldloc_S, values);
                    ilg.Emit(OpCodes.Ldc_I4, index);
                    ilg.Emit(OpCodes.Ldelem_Ref);
                    ilg.Emit(OpCodes.Stloc_S, objValue);

                    //tmpBool=Convert.IsDBNull(objValue);
                    ilg.Emit(OpCodes.Ldloc_S, objValue);
                    ilg.Emit(OpCodes.Call, Convert_IsDBNull);

                    //if (!tmpBool){
                    ilg.Emit(OpCodes.Brtrue_S, lblArray[index * 2]);

                    //item.Field=Convert(objValue).ToXXX();
                    ilg.Emit(OpCodes.Ldloc_S, item);
                    ilg.Emit(OpCodes.Ldloc_S, objValue);

                    convertValue(ilg, pi);

                    ilg.Emit(OpCodes.Callvirt, pi.GetSetMethod());
                    //}
                    ilg.Emit(OpCodes.Br_S, lblArray[index * 2 + 1]);
                    //else {
                    ilg.MarkLabel(lblArray[index * 2]);
                    //item.Field=objValue;
                    ilg.Emit(OpCodes.Ldloc_S, item);
                    ilg.Emit(OpCodes.Ldloc_S, typeNulls[index]);
                    ilg.Emit(OpCodes.Callvirt, pi.GetSetMethod());
                    //}
                    ilg.MarkLabel(lblArray[index * 2 + 1]);
                }

                //list.Add(item);
                ilg.Emit(OpCodes.Ldloc_S, list);
                ilg.Emit(OpCodes.Ldloc_S, item);
                ilg.Emit(OpCodes.Callvirt, LAdd);
                //}
                ilg.Emit(OpCodes.Br, loop);
                ilg.MarkLabel(exit);

                // return list;
                ilg.Emit(OpCodes.Ldloc_S, list);
                ilg.Emit(OpCodes.Ret);
#if WRITE_FILE
                Type t = tb.CreateType();
                ab.Save(aName.Name + ".dll");
#else
                //添加到缓存
                m_CatchMethod.Add(key, dm);
#endif
                if (m_CatchMethod.Count > 100)
                {
                    //"缓存的处理方法数据以达" + m_CatchMethod.Count;
                }
            }

            if (m_CatchMethod.ContainsKey(key))
            {
                DynamicMethod dm = m_CatchMethod[key];
                //使用代理来执行动态方法，这样比直接使用Invoke效率更高，因为无需组织一个参数数组。
                ReadEntityInvoker<List<T>> invoker = dm.CreateDelegate(typeof(ReadEntityInvoker<List<T>>)) as ReadEntityInvoker<List<T>>;
                return invoker.Invoke(reader);
            }
            throw new Exception("没有找到对应类型的处理方法。");
        }

        /// <summary>
        /// 定义对象的每一个成员类型对应的空值。
        /// </summary>
        /// <param name="properties">对象属性数组</param>
        /// <param name="ilg">代码生成器</param>
        /// <param name="typeNulls">类型对应的空值，Null或Nullable<basetype>()</param>
        private static void initNulls(PropertyInfo[] properties, ILGenerator ilg, out LocalBuilder[] typeNulls)
        {
            typeNulls = new LocalBuilder[properties.Length];
            for (int index = 0; index < properties.Length; index++)
            {
                PropertyInfo pi = properties[index];
                if (pi != null)
                {
                    typeNulls[index] = ilg.DeclareLocal(pi.PropertyType);
                    if (pi.PropertyType.IsValueType)
                    {
                        ilg.Emit(OpCodes.Ldloca_S, typeNulls[index]);
                        ilg.Emit(OpCodes.Initobj, pi.PropertyType);
                    }
                    else
                    {
                        ilg.Emit(OpCodes.Ldnull);
                        ilg.Emit(OpCodes.Stloc_S, typeNulls[index]);
                    }
                }
            }
        }

        /// <summary>
        /// 由T的属性类型决定使用的Convert方法。
        /// </summary>
        /// <param name="ilg"></param>
        /// <param name="pi"></param>
        ///
        /// <returns></returns>
        private static void convertValue(ILGenerator ilg, PropertyInfo pi)
        {
            //不可空类型
            TypeCode code = Type.GetTypeCode(pi.PropertyType);
            switch (code)
            {
                case TypeCode.Int16:
                    ilg.Emit(OpCodes.Call, Convert_ToInt16);
                    return;

                case TypeCode.Int32:
                    ilg.Emit(OpCodes.Call, Convert_ToInt32);
                    return;

                case TypeCode.Int64:
                    ilg.Emit(OpCodes.Call, Convert_ToInt64);
                    return;

                case TypeCode.Boolean:
                    ilg.Emit(OpCodes.Call, Convert_ToBoolean);
                    return;

                case TypeCode.String:
                    ilg.Emit(OpCodes.Callvirt, Object_ToString);
                    return;

                case TypeCode.DateTime:
                    ilg.Emit(OpCodes.Call, Convert_ToDateTime);
                    return;

                case TypeCode.Decimal:
                    ilg.Emit(OpCodes.Call, Convert_ToDecimal);
                    return;

                case TypeCode.Double:
                    ilg.Emit(OpCodes.Call, Convert_ToDouble);
                    return;
            }
            //可空类型处理
            Type type = Nullable.GetUnderlyingType(pi.PropertyType);
            if (type != null)
            {
                code = Type.GetTypeCode(type);
                switch (code)
                {
                    case TypeCode.Int16:
                        ilg.Emit(OpCodes.Call, Convert_ToNullInt16);
                        return;

                    case TypeCode.Int32:
                        ilg.Emit(OpCodes.Call, Convert_ToNullInt32);
                        return;

                    case TypeCode.Int64:
                        ilg.Emit(OpCodes.Call, Convert_ToNullInt64);
                        return;

                    case TypeCode.Boolean:
                        ilg.Emit(OpCodes.Call, Convert_ToNullBoolean);
                        return;

                    case TypeCode.DateTime:
                        ilg.Emit(OpCodes.Call, Convert_ToNullDateTime);
                        return;

                    case TypeCode.Decimal:
                        ilg.Emit(OpCodes.Call, Convert_ToNullDecimal);
                        return;

                    case TypeCode.Double:
                        ilg.Emit(OpCodes.Call, Convert_ToNullDouble);
                        return;
                }
            }
            throw new Exception(string.Format("不支持\"{0}\"类型的转换！", pi.PropertyType.Name));
        }

        /// <summary>
        /// 读取Reader的列与Type属性的对应关系。
        /// </summary>
        /// <param name="type">要分析的类型</param>
        /// <param name="reader">使用DataReader对象</param>
        /// <param name="mappedProerties">返回属性数组，无对应的是元素是Null。</param>
        private static void getMapped(Type type, IDataReader reader, out PropertyInfo[] mappedProerties)
        {
            mappedProerties = new PropertyInfo[reader.FieldCount];
            string[] fields = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fields[i] = reader.GetName(i);
            }
            List<PropertyInfo> properties = new List<PropertyInfo>(type.GetProperties());
            for (int i = 0; i < reader.FieldCount; i++)
            {
                foreach (PropertyInfo pt in properties)
                {
                    FieldAttribute fa = Attribute.GetCustomAttribute(pt, typeof(FieldAttribute)) as FieldAttribute;
                    if ((fa != null && string.Compare(fa.Field, fields[i], true) == 0) || string.Compare(pt.Name, fields[i], true) == 0)
                    {
                        properties.Remove(pt);
                        mappedProerties[i] = pt;
                        break;
                    }
                }
            }
        }
    }
}