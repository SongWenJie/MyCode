using CholeDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CholeDemoTest
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void Test()
        {
            UnitWork unitwork = new UnitWork();

            unitwork.BeginTransaction();

            User addUser = new User
            {
                Id = 1,
                Age = 24,
                Name = "1",
                Gender = 0,
                Status = 1
            };
            unitwork.Insert<User>(addUser);

            unitwork.Delete<User>(u => u.Id == 37);

            User user = new User
            {
                Id = 1,
                Age = 24,
               Name ="3"
            };
            unitwork.Update(user);

            unitwork.CommitTransaction();
        }


    }
}
