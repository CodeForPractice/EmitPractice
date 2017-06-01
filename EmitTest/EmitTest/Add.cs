namespace EmitTest
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：Add.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：Add
    /// 创建标识：yjq 2017/6/1 14:19:08
    /// </summary>
    public class Add

    {

        private int _a = 0;

        public int A

        {

            get { return _a; }

            set { _a = value; }

        }



        private int _b = 0;

        public int B

        {

            get { return _b; }

            set { _b = value; }

        }



        public Add(int a, int b)

        {

            _a = a;

            _b = b;

        }



        public int Calc()

        {

            return _a + _b;

        }
    }
}
