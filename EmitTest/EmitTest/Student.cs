using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitTest
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：Student.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：Student
    /// 创建标识：yjq 2017/6/1 17:05:40
    /// </summary>
    public sealed class Student
    {
        public static void SetGetAge1IL()
        {
            Console.WriteLine("---1---");
            var aa = AssemblyUtil.GetTypeBuilder("Student", (m, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = m.DefineMethod("GetAge1", System.Reflection.MethodAttributes.Public, typeof(int), Type.EmptyTypes);
                ILGenerator il = methodBuilder.GetILGenerator();
                LocalBuilder age = il.DeclareLocal(typeof(int));
                il.Emit(OpCodes.Ldc_I4_S, 10);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloc_0);

                il.Emit(OpCodes.Ret);

                Type type = m.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);
                object obj = Activator.CreateInstance(type);
                Console.WriteLine(type.GetMethod("GetAge1").Invoke(obj, null));
            });

        }

        public static void SetGetAge2IL()
        {
            Console.WriteLine("---2---");
            AssemblyUtil.GetTypeBuilder("Student", (typeBuilder, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetAge2", System.Reflection.MethodAttributes.Public, typeof(int), new Type[] { typeof(int) });

                ILGenerator il = methodBuilder.GetILGenerator();
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ret);
                Type type = typeBuilder.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);
                object obj = Activator.CreateInstance(type);
                Console.WriteLine(type.GetMethod("GetAge2").Invoke(obj, new object[] { 99 }));
            });
        }

        public static void SetGetAge3IL()
        {
            Console.WriteLine("---3---");
            AssemblyUtil.GetTypeBuilder("Student", (typeBuilder, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetAge3", System.Reflection.MethodAttributes.Public, typeof(int), new Type[] { typeof(int) });

                ILGenerator il = methodBuilder.GetILGenerator();
                Label returnLable1 = il.DefineLabel();
                Label lbReturnResutl = il.DefineLabel();
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Clt);
                il.Emit(OpCodes.Brtrue_S, returnLable1);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4_S,100);
                il.Emit(OpCodes.Cgt);
                il.Emit(OpCodes.Brtrue_S, returnLable1);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Br, lbReturnResutl);

                il.MarkLabel(returnLable1);
                il.Emit(OpCodes.Ldc_I4_1);

                il.MarkLabel(lbReturnResutl);
               
                il.Emit(OpCodes.Ret);

                Type type = typeBuilder.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);
                object obj = Activator.CreateInstance(type);
                Console.WriteLine(type.GetMethod("GetAge3").Invoke(obj, new object[] { 99 }));
                Console.WriteLine(type.GetMethod("GetAge3").Invoke(obj, new object[] { 101 }));
                Console.WriteLine(type.GetMethod("GetAge3").Invoke(obj, new object[] { -1 }));
                Console.WriteLine(type.GetMethod("GetAge3").Invoke(obj, new object[] { 50 }));
                Console.WriteLine(type.GetMethod("GetAge3").Invoke(obj, new object[] { 100 }));
                Console.WriteLine(type.GetMethod("GetAge3").Invoke(obj, new object[] { 33 }));
            });
        }
    }
}
