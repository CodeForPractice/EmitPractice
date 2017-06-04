using System;
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
        public delegate PropertyInfo[] LoadPropertiesDelegate(BindingFlags bindingFlags);

        public static PropertyInfo[] GetTypeProperties(Type type)
        {
            AssemblyName aName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
            TypeBuilder tb = mb.DefineType("GetProperty", TypeAttributes.Public);
            MethodBuilder method = tb.DefineMethod("GetProperties", MethodAttributes.Public | MethodAttributes.Static, typeof(PropertyInfo[]), new Type[] { typeof(BindingFlags) });


            var methodInfo = typeof(Type).GetMethod("GetProperties", Type.EmptyTypes);
            //DynamicMethod method = new DynamicMethod("GetProperty" + type.Name, typeof(PropertyInfo[]), new Type[] { typeof(BindingFlags) });
            ILGenerator il = method.GetILGenerator();
            LocalBuilder typeInstance = il.DeclareLocal(typeof(Type));
            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, typeInstance);
            il.Emit(OpCodes.Ldloc, typeInstance);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, methodInfo);
            il.Emit(OpCodes.Ret);
            Type t = tb.CreateType();
            ab.Save(aName.Name + ".dll");

            object obj = Activator.CreateInstance(t);
           return t.GetMethod("GetProperties").Invoke(obj,new object[] { BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic } ) as PropertyInfo[];

            //var invoke = (LoadPropertiesDelegate)method.CreateDelegate(typeof(LoadPropertiesDelegate));
            //return invoke(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static IEnumerable<PropertyInfo> GetTypeProperties1()
        {
            Type type = typeof(PropertyInfo);
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}