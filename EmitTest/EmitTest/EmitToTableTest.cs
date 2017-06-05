using Infrastructure;

namespace EmitTest
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：EmitToTableTest.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/6/5 16:02:41
    /// </summary>
    public sealed class EmitToTableTest : IAction
    {
        public void Action()
        {
            var testTable = TableTest.GetDataSet().Tables[0];
            Infrastructure.DataTableUtil.ToList<User>(testTable);
        }
    }
}