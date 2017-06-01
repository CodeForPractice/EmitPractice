using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Say()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i.ToString());
            }
        }

        public void Say(string msg)
        {
            Console.WriteLine(msg);
        }

        public int Calculate(int a, int b)
        {
            if (a < 10)
            {
                return b;
            }
            if (b < 10)
            {
                return a;
            }
            if (a == b || a + b < 10)
            {
                return 10;
            }
            return a - b;
        }
    }
}
