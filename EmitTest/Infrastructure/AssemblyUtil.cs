using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：AssemblyUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：AssemblyUtil
    /// 创建标识：yjq 2017/6/1 19:46:16
    /// </summary>
    public static class AssemblyUtil
    {
        public static string name = "Runtime.Assembly";
        public static TypeBuilder GetTypeBuilder(string typeName, Action<TypeBuilder, AssemblyBuilder> action)
        {

            AssemblyName assemblyName = new AssemblyName(name);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);
            action(typeBuilder, assemblyBuilder);
            return typeBuilder;
        }


    }
}