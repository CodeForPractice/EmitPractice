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
            table.Columns.Add("Id", typeof(object));
            table.Columns.Add("Name", typeof(object));
            table.Columns.Add("Sex", typeof(object));
            table.Columns.Add("Uid", typeof(object));
            table.Columns.Add("Time", typeof(object));
            table.Columns.Add("SourcePort", typeof(object));
            table.Columns.Add("多出来的属性", typeof(string));
            for (int i = 0; i < 20; i++)
            {
                table.Rows.Add(i, "blqw" + i, true, Guid.NewGuid(), DateTime.Now, i < 10 ? Port.APP : Port.PC, "多余的");
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
        public int? Id { get; set; }
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

        public override string ToString()
        {
            return $"{Id},{Name},{Sex},{Uid},{Time},{SexText},{SourcePort}";
        }
    }

    public enum Port
    {
        PC,
        APP
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime AddTime { get; set; }

        public override string ToString()
        {
            return $"{Id},{Name},{Price},{AddTime}";
        }
    }
}
