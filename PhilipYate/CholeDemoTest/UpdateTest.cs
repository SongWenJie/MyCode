using CholeDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CholeDemoTest
{
    [TestClass]
    public class UpdateTest : BaseRepository<User>
    {
        [TestMethod]
        public void TestEntityUpdate()
        {
            User user = new User
            {
                Id = 1,
                Name = "EntityUpdate",
                Age = 20,
                Gender = 1,
                Status = 2
            };

            bool result = Update(user);

            Assert.IsTrue(result);
        }


        [TestMethod]
        public void TestLambdaUpdate()
        {
            bool result = Update(u => u.Id ==1,
                u => new User
                {
                    Name = "TestLambdaUpdate",
                    Age = u.Age + 10
                });

            Assert.IsTrue(result);
        }
    }
}
