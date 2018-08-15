using DapperDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DapperDemoTest
{
    [TestClass]
    public class ExcuteTest
    {
        [TestMethod]
        public void TestSingleInsert()
        {
            string sql = "insert into user (name,age,gender,status) values (?name,?age,?gender,?status);";
            var param = new { name = "dapper", age = 2, gender = 0, status = 1 };

            int count = DapperDemo.Dapper.Execute(sql, param);

            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        public void TestMultipleInsert()
        {
            string sql = "insert into user (name,age,gender,status) values (?name,?age,?gender,?status);";
            var param = new[]
                {
                    new { name = "dapper1", age = 2, gender = 0, status = 1 },
                    new { name = "dapper2", age = 2, gender = 0, status = 1 },
                    new { name = "dapper3", age = 2, gender = 0, status = 1 }
                };

            int count = DapperDemo.Dapper.Execute(sql, param);

            Assert.IsTrue(count == 3);
        }

        [TestMethod]
        public void TestUpdate()
        {
            string sql = "update  user set remark=?remark where id=?id;";
            var param = new { remark = "dapper update",id=1 };

            int count = DapperDemo.Dapper.Execute(sql, param);

            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        public void TestMulUpdate()
        {
            string sql = "update  user set remark=?remark where id=?id;";
            var param =
                new[]
                {
                    new { remark = "dapper update1", id = 1 },
                    new { remark = "dapper update2", id = 2 },
                    new { remark = "dapper update3", id = 3 },
                };

            int count = DapperDemo.Dapper.Execute(sql, param);

            Assert.IsTrue(count == 3);
        }


        [TestMethod]
        public void TestDelete()
        {
            string sql = "delete from user where name=?name;";
            var param = new { name = "dapper" };
               

            int count = DapperDemo.Dapper.Execute(sql, param);

            Assert.IsTrue(count >0);
        }

    }
}
