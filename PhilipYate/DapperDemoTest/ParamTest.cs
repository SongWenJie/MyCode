using DapperDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDemoTest
{
    [TestClass]
    public class ParamTest
    {
        /// <summary>
        /// dapper make it sample and safe (SQL Injection) 
        /// to use parameter by supporting anonymous type
        /// </summary>
        [TestMethod]
        public void TestUseAnonymousParameter()
        {
            string sql = "select * from user where id = ?id;";
            var param = new { id = 1 };

            var user = DapperDemo.Dapper.Query<User>(sql, param);
        }

        /// <summary>
        /// dapper allow you to specify multiple parameter
        /// on a In clause by using a list
        /// </summary>
        [TestMethod]
        public void TestUseListParameter()
        {
            string sql = "select * from user where id in ?id;";
            var param = new { id= new[] { 1, 2, 3 } };

            var user = DapperDemo.Dapper.Query<User>(sql, param);
        }



    }
}
