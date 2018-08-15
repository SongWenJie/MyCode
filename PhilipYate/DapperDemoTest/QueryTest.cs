using DapperDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperDemoTest
{
    [TestClass]
    public class QueryTest
    {

        #region QueryFirst
        [TestMethod]
        public void TestQueryFirstNoItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = -1 };

            Assert.ThrowsException<InvalidOperationException>(()
                =>
            {
                DapperDemo.Dapper.QueryFirst<User>(sql, param);
            });
        }

        [TestMethod]
        public void TestQueryFirstOneItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = 1 };

            var user = DapperDemo.Dapper.QueryFirst<User>(sql, param);

            Assert.IsTrue(user.Id == 1);
        }

        [TestMethod]
        public void TestQueryFirstManyItems()
        {
            string sql = "select * from user where gender=?gender;";
            var param = new { gender = 0 };

            var user = DapperDemo.Dapper.QueryFirst<User>(sql, param);

            Assert.IsTrue(user.Id == 2);
        }
        #endregion

        #region QuerySingle
        [TestMethod]
        public void TestQuerySingleNoItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = -1 };

            Assert.ThrowsException<InvalidOperationException>(()
                =>
            {
                DapperDemo.Dapper.QuerySingle<User>(sql, param);
            });
        }


        [TestMethod]
        public void TestQuerySingleOneItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = 1 };

            var user = DapperDemo.Dapper.QuerySingle<User>(sql, param);

            Assert.IsTrue(user.Id == 1);
        }

        [TestMethod]
        public void TestQuerySingleManyItems()
        {
            string sql = "select * from user where gender=?gender;";
            var param = new { gender = 0 };

            Assert.ThrowsException<InvalidOperationException>(()
                =>
            {
                DapperDemo.Dapper.QuerySingle<User>(sql, param);
            });
        }
        #endregion

        #region QueryFirstOrDefault
        [TestMethod]
        public void TestQueryFirstOrDefaultNoItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = -1 };

            var user = DapperDemo.Dapper.QueryFirstOrDefault<User>(sql, param);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void TestQueryFirstOrDefaultOneItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = 1 };

            var user = DapperDemo.Dapper.QueryFirstOrDefault<User>(sql, param);

            Assert.AreEqual(1, user.Id);
        }

        [TestMethod]
        public void TestQueryFirstOrDefaultManyItems()
        {
            string sql = "select * from user where id in(?id);";
            var param = new { id = "2,1,3" };

            var user = DapperDemo.Dapper.QueryFirstOrDefault<User>(sql, param);

            Assert.AreEqual(2, user.Id);
        }
        #endregion

        #region QuerySingleOrDefault
        [TestMethod]
        public void TestQuerySingleOrDefaultNoItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = -1 };

            var user = DapperDemo.Dapper.QuerySingleOrDefault<User>(sql, param);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void TestQuerySingleOrDefaultOneItem()
        {
            string sql = "select * from user where id=?id;";
            var param = new { id = 1 };

            var user = DapperDemo.Dapper.QuerySingleOrDefault<User>(sql, param);

            Assert.AreEqual(1, user.Id);
        }

        [TestMethod]
        public void TestQuerySingleOrDefaultManyItems()
        {
            string sql = "select * from user ;";

            Assert.ThrowsException<InvalidOperationException>(()
               =>
            {
                DapperDemo.Dapper.QuerySingleOrDefault<User>(sql, null);
            });
        }
        #endregion

        #region QueryMultiple
        [TestMethod]
        public void TestQueryMultiple()
        {
            string sql = @"select * from user where id = ?id;
                                    select * from actor where actor_id=?actor_id;";
            var param = new { id = 1, actor_id = 1 };

            using (var mul = DapperDemo.Dapper.QueryMultiple(sql, param))
            {
                var users = mul.Read<User>().ToList();
                var actors = mul.Read<Actor>().ToList();
            }
        }
        #endregion

        #region  Query Multi-Mapping
        /// <summary>
        /// Extension methods can be used to excute a query 
        /// and mapp the result to a strongly typed list with relations.
        /// This method show the One to One relation
        /// </summary>
        [TestMethod]
        public void TestQueryMultiMappingOnetoOne()
        {
            string sql = @"select * from user u inner join user_ext ext on u.id = ext.id;";

            var result = DapperDemo.Dapper.Query<User, User_Ext, User>(
                sql, (user, user_ext) =>
                {
                    user.user_ext = user_ext;
                    return user;
                }, null, splitOn: "id");
        }

        /// <summary>
        /// Extension methods can be used to excute a query 
        /// and mapp the result to a strongly typed list with relations.
        /// This method show the One to Many relation
        /// If you get this,you can give up the SQL group method
        /// </summary>
        [TestMethod]
        public void TestQueryMultiMappingOnetoMany()
        {
            string sql = @"select * from customer c inner join payment p 
                                    on c.customer_id = p.customer_id;";

            Dictionary<int, Customer> dic = new Dictionary<int, Customer>();

            var result = DapperDemo.Dapper.Query<Customer, Payment, Customer>(
                sql, (customer, payment) =>
                {
                    Customer cus;
                    if (!dic.TryGetValue(customer.customer_id, out cus))
                    {
                        cus = customer;
                        cus.Payments = new List<Payment>();
                        dic.Add(cus.customer_id, cus);
                    }
                    cus.Payments.Add(payment);
                    return cus;
                }, null, splitOn: "payment_id").Distinct();
        }

        #endregion

    }
}
