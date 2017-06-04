using Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
                il.Emit(OpCodes.Ldc_I4_S, 100);
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

        public static void SetGetAgeIL()
        {
            Console.WriteLine("---4---");
            AssemblyUtil.GetTypeBuilder("Student", (typeBuilder, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetAge", System.Reflection.MethodAttributes.Public, typeof(int), Type.EmptyTypes);

                ILGenerator il = methodBuilder.GetILGenerator();
                LocalBuilder local1 = il.DeclareLocal(typeof(int));
                Label lableReturn = il.DefineLabel();
                Label lableFinally = il.DefineLabel();
                il.Emit(OpCodes.Ldc_I4_S, 10);
                il.Emit(OpCodes.Stloc_0);

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldc_I4_S, 100);
                il.Emit(OpCodes.Blt_S, lableReturn);
                //il.Emit(OpCodes.Brtrue_S, lableReturn);

                il.Emit(OpCodes.Ldc_I4_S, 100);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Br, lableFinally);

                il.MarkLabel(lableReturn);
                il.Emit(OpCodes.Ldloc_0);

                il.MarkLabel(lableFinally);

                il.Emit(OpCodes.Ret);

                Type type = typeBuilder.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);
                object obj = Activator.CreateInstance(type);
                Console.WriteLine(type.GetMethod("GetAge").Invoke(obj, null));
            });
        }

        public static void SetSayMsgIL()
        {
            Console.WriteLine("---5---");
            AssemblyUtil.GetTypeBuilder("Student", (typeBuilder, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("Say", System.Reflection.MethodAttributes.Public, null, new Type[] { typeof(string) });

                ILGenerator il = methodBuilder.GetILGenerator();
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                il.Emit(OpCodes.Ret);

                Type type = typeBuilder.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);
                object obj = Activator.CreateInstance(type);
                type.GetMethod("Say").Invoke(obj, new object[] { "Hellow World" });
            });
        }

        public static void SetCalculateIL()
        {
            Console.WriteLine("---6---");
            AssemblyUtil.GetTypeBuilder("Student", (typeBuilder, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("Calculate", System.Reflection.MethodAttributes.Public, typeof(int), new Type[] { typeof(int), typeof(int) });

                ILGenerator il = methodBuilder.GetILGenerator();
                LocalBuilder local1 = il.DeclareLocal(typeof(int));
                Label lable1 = il.DefineLabel();
                Label lable2 = il.DefineLabel();
                Label lable3 = il.DefineLabel();
                Label lableFinally = il.DefineLabel();

                il.Emit(OpCodes.Ldc_I4_S, 10);
                il.Emit(OpCodes.Stloc_0);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Blt_S, lable1);

                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Clt);
                il.Emit(OpCodes.Brtrue_S, lable2);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Ceq);
                il.Emit(OpCodes.Brtrue_S, lable3);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Clt);
                il.Emit(OpCodes.Brtrue_S, lable3);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Br_S, lableFinally);

                il.MarkLabel(lable1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Br_S, lableFinally);

                il.MarkLabel(lable2);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Br_S, lableFinally);


                il.MarkLabel(lable3);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Br_S, lableFinally);

                il.MarkLabel(lableFinally);

                il.Emit(OpCodes.Ret);

                Type type = typeBuilder.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);
                object obj = Activator.CreateInstance(type);
                Console.WriteLine(type.GetMethod("Calculate").Invoke(obj, new object[] { 5, 10 }));
                Console.WriteLine(type.GetMethod("Calculate").Invoke(obj, new object[] { 4, 4 }));
                Console.WriteLine(type.GetMethod("Calculate").Invoke(obj, new object[] { 20, 20 }));
                Console.WriteLine(type.GetMethod("Calculate").Invoke(obj, new object[] { 20, 30 }));
            });
        }

        public static void SetSayIL()
        {
            Console.WriteLine("---7---");
            AssemblyUtil.GetTypeBuilder("Student", (typeBuilder, assemblyBuilder) =>
            {
                MethodBuilder methodBuilder = typeBuilder.DefineMethod("Say", System.Reflection.MethodAttributes.Public, null, Type.EmptyTypes);

                ILGenerator il = methodBuilder.GetILGenerator();

                LocalBuilder localI = il.DeclareLocal(typeof(int));
                LocalBuilder localLength = il.DeclareLocal(typeof(int));
                LocalBuilder local3 = il.DeclareLocal(typeof(int));
                Label forStartLable = il.DefineLabel();
                Label finallyLable = il.DefineLabel();
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldc_I4_S, 10);
                il.Emit(OpCodes.Stloc_1);
                il.MarkLabel(forStartLable);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Clt);
                il.Emit(OpCodes.Stloc_2);
                il.Emit(OpCodes.Ldloc_2);
                il.Emit(OpCodes.Brfalse_S, finallyLable);

                //il.Emit(OpCodes.Ldloca_S, localI);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Call, typeof(Program).GetMethod("Show", new Type[] { typeof(int) }));
                //il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
                //il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Br_S, forStartLable);

                il.MarkLabel(finallyLable);
                il.Emit(OpCodes.Ret);

                Type type = typeBuilder.CreateType();
                assemblyBuilder.Save(AssemblyUtil.name);
                object obj = Activator.CreateInstance(type);
                type.GetMethod("Say").Invoke(obj, null);
            });


        }


        public static void ToList()
        {
            User user = new User
            {
                Id = 1

            };
            object product = new Product
            {
                ProductAddTime = DateTime.Now,
                ProductId = 1
            };

            user.Product = (Product)product;
        }
    }
}
