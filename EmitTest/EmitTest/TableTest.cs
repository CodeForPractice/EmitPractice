using Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitTest
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：TableTest.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：TableTest
    /// 创建标识：yjq 2017/6/2 19:40:19
    /// </summary>
    public sealed class TableTest
    {

        public static void Test1()
        {
            //DataTable table = new DataTable();
            //table.Columns.Add("id", typeof(int));
            //table.Columns.Add("name",typeof(string));
            //table.Columns.Add("price", typeof(decimal));
            //table.Columns.Add("AddTime", typeof(DateTime));
            //DataRow row = table.NewRow();
            //row[0] = 9;
            //row[1] = "电脑";
            //row[2] = 12.25;
            //row[3] = DateTime.Now;
            //table.Rows.Add(row);
            //Stopwatch watch = Stopwatch.StartNew();
            //for (int i = 0; i < 10; i++)
            //{
            //    var result = DataTableUtil.ToT<Product>(row);
            //    //var result = DataTableUtil.ToEntity<Product>(row);
            //    Console.WriteLine(result.ToString());
            //}
            //Console.WriteLine(watch.ElapsedMilliseconds.ToString());
            //watch.Stop();

            var testTable = GetDataSet().Tables[0];
            foreach (DataRow item in testTable.Rows)
            {
                var result = DataTableUtil.ToT<User>(item);
                Console.WriteLine(result.ToString());
            }
        }
        //模拟方法
        static public DataSet GetDataSet()
        {
            DataTable table = new DataTable("User");
            table.Columns.Add("Id");
            table.Columns.Add("Name");
            table.Columns.Add("Sex");
            table.Columns.Add("Uid");
            table.Columns.Add("Time");
            table.Columns.Add("SourcePort", typeof(object));
            table.Columns.Add("ProductId");
            table.Columns.Add("ProductName");
            table.Columns.Add("ProductPrice");
            table.Columns.Add("ProductAddTime", typeof(object));
            for (int i = 0; i < 20; i++)
            {
                table.Rows.Add(i, "blqw" + i, true, Guid.NewGuid(), DateTime.Now, i < 10 ? Port.APP : Port.PC, i, "product" + i.ToString(), i, DateTime.Now);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ds;
        }
    }

    public class User
    {
        public User()
        {

        }
        public double? Id { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }
        public Guid Uid { get; set; }
        public Port SourcePort { get; set; }
        public DateTime? Time { get; set; }
        public string SexText
        {
            get
            {
                return Sex ? "男" : "女";
            }
            set
            {
                Sex = (value == "男");
            }
        }

        public Product Product { get; set; }
        public override string ToString()
        {
            return $"{Id},{Name},{Sex},{Uid},{Time},{SexText},{SourcePort},{Product?.ToString()}";
        }
    }

    public enum Port
    {
        PC,
        APP
    }

    public class Product
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public DateTime ProductAddTime { get; set; }

        public override string ToString()
        {
            return $"{ProductId},{ProductName},{ProductPrice},{ProductAddTime}";
        }
    }
}
