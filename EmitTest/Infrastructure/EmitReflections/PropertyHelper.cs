using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure.EmitReflections
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：PropertyUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：PropertyUtil
    /// 创建标识：yjq 2017/6/4 14:44:11
    /// </summary>
    public static class PropertyHelper
    {
        private delegate PropertyInfo[] LoadPropertiesDelegate(Type type, BindingFlags bindingFlags);

        private static ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod> _PropertyMethodCache = new ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod>();

        private static DynamicMethod CreateGetPropertiesMethod(Type type)
        {
            var methodInfo = typeof(Type).GetMethod("GetProperties", new Type[] { typeof(BindingFlags) });
            DynamicMethod method = new DynamicMethod("GetProperty" + type.Name, typeof(PropertyInfo[]), new Type[] { typeof(Type), typeof(BindingFlags) });
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, methodInfo);
            il.Emit(OpCodes.Ret);
            return method;
        }

        public static PropertyInfo[] GetTypeProperties(Type type)
        {
            var method = _PropertyMethodCache.GetValue(type.TypeHandle, () =>
            {
                return CreateGetPropertiesMethod(type);
            });
            var invoke = (LoadPropertiesDelegate)method.CreateDelegate(typeof(LoadPropertiesDelegate));
            return invoke(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static IEnumerable<PropertyInfo> GetTypeProperties1(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}