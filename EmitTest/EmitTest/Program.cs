using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace EmitTest
{
    public class Program
    {
        public void Show(int a)
        {
            Console.WriteLine("Show" + a.ToString());
        }


        //C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin
        private delegate void HelloWorldDelegate();

        private static void Main(string[] args)
        {
            Infrastructure.EmitReflections.PropertyHelper.GetTypeProperties(typeof(User));

            //List<User> userList = new List<EmitTest.User>();
            //for (int i = 0; i < 10; i++)
            //{
            //    User user = new EmitTest.User();
            //    user.Name = "你好" + i.ToString();
            //    user.Id = i;
            //    userList.Add(user);
            //}
            //foreach (var item in userList)
            //{
            //    Console.WriteLine(item.ToString());
            //}

            //var properties = Infrastructure.EmitReflections.PropertyHelper.GetTypeProperties(typeof(User));
            //foreach (var item in properties)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //object i = 1;
            //Convert.ToDecimal(i);
            //Method1();
            //Method2();
            //Method3();

            //Student.SetGetAge1IL();
            //Student.SetGetAge2IL();
            //Student.SetGetAge3IL();
            //Student.SetGetAgeIL();
            //Student.SetSayMsgIL();
            //Student.SetCalculateIL();
            //Student.SetSayIL();
            //Student.ToList();
            //TableTest.Test1();

            Console.Read();
        }

        private static void Method4()
        {
        }

        private static void Method3()
        {
            Console.WriteLine("----3----");
            string name = "EmitTest.Test";
            string asmFileName = name + ".dll";

            #region Step 1 构建程序集

            //创建程序集名
            AssemblyName asmName = new AssemblyName(name);

            //获取程序集所在的应用程序域
            //你也可以选择用AppDomain.CreateDomain方法创建一个新的应用程序域
            //这里选择当前的应用程序域
            AppDomain domain = AppDomain.CurrentDomain;

            //实例化一个AssemblyBuilder对象来实现动态程序集的构建
            AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);

            #endregion Step 1 构建程序集

            #region Step 2 定义模块

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name, asmFileName);

            #endregion Step 2 定义模块

            #region Step 3 定义类型

            TypeBuilder typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public);

            #endregion Step 3 定义类型

            FieldBuilder fieldABuilder = typeBuilder.DefineField("_a", typeof(Int32), FieldAttributes.Private);
            FieldBuilder fieldBBuilder = typeBuilder.DefineField("_b", typeof(Int32), FieldAttributes.Private);
            fieldABuilder.SetConstant(0);
            fieldBBuilder.SetConstant(0);

            PropertyBuilder propertyABuilder = typeBuilder.DefineProperty("A", PropertyAttributes.None, typeof(int), null);
            PropertyBuilder propertyBBuilder = typeBuilder.DefineProperty("B", PropertyAttributes.None, typeof(int), null);

            MethodBuilder getPropertyABuilder = typeBuilder.DefineMethod("get", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(int), Type.EmptyTypes);
            ILGenerator getAIL = getPropertyABuilder.GetILGenerator();
            getAIL.Emit(OpCodes.Ldarg_0);
            getAIL.Emit(OpCodes.Ldfld, fieldABuilder);
            getAIL.Emit(OpCodes.Ret);

            MethodBuilder setPropertyABuilder = typeBuilder.DefineMethod("set", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { typeof(int) });
            ILGenerator setAIL = setPropertyABuilder.GetILGenerator();
            setAIL.Emit(OpCodes.Ldarg_0);
            setAIL.Emit(OpCodes.Ldarg_1);
            setAIL.Emit(OpCodes.Stfld, fieldBBuilder);
            setAIL.Emit(OpCodes.Ret);

            propertyABuilder.SetGetMethod(getPropertyABuilder);
            propertyABuilder.SetSetMethod(setPropertyABuilder);

            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[] { typeof(int), typeof(int) });

            ILGenerator ctorIL = constructorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, fieldABuilder);

            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, fieldBBuilder);
            ctorIL.Emit(OpCodes.Ret);

            MethodBuilder calcMethodBuilder = typeBuilder.DefineMethod("Calc", MethodAttributes.Public, typeof(int), Type.EmptyTypes);
            ILGenerator calcIL = calcMethodBuilder.GetILGenerator();
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldfld, fieldABuilder);
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldfld, fieldBBuilder);

            calcIL.Emit(OpCodes.Add);
            calcIL.Emit(OpCodes.Ret);
            Type type = typeBuilder.CreateType();
            assemblyBuilder.Save(asmFileName);
            for (int i = 0; i < 10; i++)
            {
                object ob = Activator.CreateInstance(type, new object[] { i, i + 1 });
                Console.WriteLine(type.GetMethod("Calc").Invoke(ob, null).ToString());
            }
        }

        private static void Method2()
        {
            Console.WriteLine("----2----");
            string name = "EmitTest.Test";
            string asmFileName = name + ".dll";

            #region Step 1 构建程序集

            //创建程序集名
            AssemblyName asmName = new AssemblyName(name);

            //获取程序集所在的应用程序域
            //你也可以选择用AppDomain.CreateDomain方法创建一个新的应用程序域
            //这里选择当前的应用程序域
            AppDomain domain = AppDomain.CurrentDomain;

            //实例化一个AssemblyBuilder对象来实现动态程序集的构建
            AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);

            #endregion Step 1 构建程序集

            #region Step 2 定义模块

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name, asmFileName);

            #endregion Step 2 定义模块

            #region Step 3 定义类型

            TypeBuilder typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public);

            #endregion Step 3 定义类型

            #region Step 4 定义方法

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "Calc",
                MethodAttributes.Public,
                typeof(Int32),
                new Type[] { typeof(Int32) });

            #endregion Step 4 定义方法

            #region Step 5 实现方法

            ILGenerator calcIL = methodBuilder.GetILGenerator();

            //定义标签lbReturn1，用来设置返回值为1
            Label lbReturn1 = calcIL.DefineLabel();
            //定义标签lbReturnResutl，用来返回最终结果
            Label lbReturnResutl = calcIL.DefineLabel();

            //加载参数1，和整数1，相比较，如果相等则设置返回值为1
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_1);
            calcIL.Emit(OpCodes.Beq_S, lbReturn1);

            //加载参数1，和整数2，相比较，如果相等则设置返回值为1
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_2);
            calcIL.Emit(OpCodes.Beq_S, lbReturn1);

            //加载参数0和1，将参数1减去1，递归调用自身
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_1);
            calcIL.Emit(OpCodes.Sub);
            calcIL.Emit(OpCodes.Call, methodBuilder);

            //加载参数0和1，将参数1减去2，递归调用自身
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_2);
            calcIL.Emit(OpCodes.Sub);
            calcIL.Emit(OpCodes.Call, methodBuilder);

            //将递归调用的结果相加，并返回
            calcIL.Emit(OpCodes.Add);
            calcIL.Emit(OpCodes.Br, lbReturnResutl);

            //在这里创建标签lbReturn1
            calcIL.MarkLabel(lbReturn1);
            calcIL.Emit(OpCodes.Ldc_I4_1);

            //在这里创建标签lbReturnResutl
            calcIL.MarkLabel(lbReturnResutl);
            calcIL.Emit(OpCodes.Ret);

            #endregion Step 5 实现方法

            #region Step 6 收获

            Type type = typeBuilder.CreateType();

            assemblyBuilder.Save(asmFileName);

            object ob = Activator.CreateInstance(type);

            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine(type.GetMethod("Calc").Invoke(ob, new object[] { i }));
            }

            #endregion Step 6 收获
        }

        private static void Method1()
        {
            Console.WriteLine("----1----");
            DynamicMethod helloWorldMethod = new DynamicMethod("HelloWorld", null, null);
            //创建一个MSIL生成器，为动态方法生成代码
            ILGenerator helloWorldIL = helloWorldMethod.GetILGenerator();

            //将要输出的Hello World!字符创加载到堆栈上
            helloWorldIL.Emit(OpCodes.Ldstr, "Hello World!");
            //调用Console.WriteLine(string)方法输出Hello World!
            helloWorldIL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            //方法结束，返回
            helloWorldIL.Emit(OpCodes.Ret);

            //完成动态方法的创建，并且获取一个可以执行该动态方法的委托
            HelloWorldDelegate HelloWorld = (HelloWorldDelegate)helloWorldMethod.CreateDelegate(typeof(HelloWorldDelegate));

            //执行动态方法，将在屏幕上打印Hello World!
            HelloWorld();
        }
    }
}