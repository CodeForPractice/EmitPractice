using System;
using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：MethodUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/6/3 16:17:38
    /// </summary>
    public static class MethodUtil
    {
        private static Dictionary<RuntimeTypeHandle, MethodInfo> _methodCache = null;

        static MethodUtil()
        {
            _methodCache = new Dictionary<RuntimeTypeHandle, MethodInfo>()
            {
                [typeof(byte).TypeHandle] = typeof(ConvertUtil).GetMethod("ToByte", new Type[] { typeof(object) }),
                [typeof(sbyte).TypeHandle] = typeof(ConvertUtil).GetMethod("ToSByte", new Type[] { typeof(object) }),
                [typeof(short).TypeHandle] = typeof(ConvertUtil).GetMethod("ToInt16", new Type[] { typeof(object) }),
                [typeof(ushort).TypeHandle] = typeof(ConvertUtil).GetMethod("ToUInt16", new Type[] { typeof(object) }),
                [typeof(int).TypeHandle] = typeof(ConvertUtil).GetMethod("ToInt32", new Type[] { typeof(object) }),
                [typeof(uint).TypeHandle] = typeof(ConvertUtil).GetMethod("ToUInt32", new Type[] { typeof(object) }),
                [typeof(long).TypeHandle] = typeof(ConvertUtil).GetMethod("ToInt64", new Type[] { typeof(object) }),
                [typeof(ulong).TypeHandle] = typeof(ConvertUtil).GetMethod("ToUInt64", new Type[] { typeof(object) }),
                [typeof(bool).TypeHandle] = typeof(ConvertUtil).GetMethod("ToBoolean", new Type[] { typeof(object) }),
                [typeof(decimal).TypeHandle] = typeof(ConvertUtil).GetMethod("ToDecimal", new Type[] { typeof(object) }),
                [typeof(double).TypeHandle] = typeof(ConvertUtil).GetMethod("ToDouble", new Type[] { typeof(object) }),
                [typeof(float).TypeHandle] = typeof(ConvertUtil).GetMethod("ToFloat", new Type[] { typeof(object) }),
                [typeof(char).TypeHandle] = typeof(ConvertUtil).GetMethod("ToChar", new Type[] { typeof(object) }),
                [typeof(Guid).TypeHandle] = typeof(ConvertUtil).GetMethod("ToGuid", new Type[] { typeof(object) }),
                [typeof(DateTime).TypeHandle] = typeof(ConvertUtil).GetMethod("ToDateTime", new Type[] { typeof(object) }),
                [typeof(byte?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNByte", new Type[] { typeof(object) }),
                [typeof(sbyte?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNSByte", new Type[] { typeof(object) }),
                [typeof(short?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNInt16", new Type[] { typeof(object) }),
                [typeof(ushort?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNUInt16", new Type[] { typeof(object) }),
                [typeof(int?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNInt32", new Type[] { typeof(object) }),
                [typeof(uint?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNUInt32", new Type[] { typeof(object) }),
                [typeof(long?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNInt64", new Type[] { typeof(object) }),
                [typeof(ulong?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNUInt64", new Type[] { typeof(object) }),
                [typeof(bool?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNBoolean", new Type[] { typeof(object) }),
                [typeof(decimal?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNDecimal", new Type[] { typeof(object) }),
                [typeof(double?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNDouble", new Type[] { typeof(object) }),
                [typeof(float?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNFloat", new Type[] { typeof(object) }),
                [typeof(char?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNChar", new Type[] { typeof(object) }),
                [typeof(Guid?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNGuid", new Type[] { typeof(object) }),
                [typeof(DateTime?).TypeHandle] = typeof(ConvertUtil).GetMethod("ToNDateTime", new Type[] { typeof(object) })
            };
        }

        public static void Add(RuntimeTypeHandle typeHandle, MethodInfo methodInfo)
        {
            if (!_methodCache.ContainsKey(typeHandle))
            {
                _methodCache[typeHandle] = methodInfo;
            }
        }

        private static MethodInfo GetMethodInfo(RuntimeTypeHandle typeHandle)
        {
            if (_methodCache.ContainsKey(typeHandle))
            {
                return _methodCache[typeHandle];
            }
            return null;
        }

        public static MethodInfo GetMethodInfo(Type type)
        {
            //if (type.IsEnum)
            //{
            //    return GetMethodInfo(typeof(int).TypeHandle);
            //}
            if (_methodCache.ContainsKey(type.TypeHandle))
            {
                return _methodCache[type.TypeHandle];
            }
            return null;
        }
    }
}