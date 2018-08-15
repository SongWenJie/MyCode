using Chloe;
using CholeDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CholeDemoTest
{
    [TestClass]
    public class InsertTest: BaseRepository<User>
    {
        [TestMethod]
        public void TestEntityInsert()
        {
            User user = new User
            {
                Name = "YaTe",
                Age = 20,
                Gender = 1,
                Status = 2
            };

            var result = Insert(user);
        }

        [TestMethod]
        public void TestLambdaInsert()
        {
            bool result = Insert1(() =>
            new User
            {
                Name = "lambda",
                Age = 1,
                Gender = 0,
                Status = 1
            });
        }
    }
}
