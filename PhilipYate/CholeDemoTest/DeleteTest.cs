using CholeDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CholeDemoTest
{
    [TestClass]
    public class DeleteTest: BaseRepository<User>
    {
        [TestMethod]
        public void TestEntityDelete()
        {
            User user = new User
            {
                Id = 34
            };

            bool result = Delete(user);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLambdaDelete()
        {
            bool result = Delete(u=>u.Age <= 10 );

            Assert.IsTrue(result);
        }
    }
}
