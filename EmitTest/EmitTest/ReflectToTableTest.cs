using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmitTest
{
    /// <summary>
    /// Copyright (C) 2015 备胎 版权所有。
    /// 类名：ReflectToTableTest.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/6/5 16:04:12
    /// </summary>
    public sealed class ReflectToTableTest:IAction
    {
        public void Action()
        {
            var testTable = TableTest.GetDataSet().Tables[0];
            Infrastructure.DataTableUtil.ToEntities<User>(testTable);
        }
    }
}
