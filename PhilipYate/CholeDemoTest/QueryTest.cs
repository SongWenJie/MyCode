using CholeDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CholeDemoTest
{
    /// <summary>
    /// 延迟执行，返回 IQuery<T>时可以看到生成的sql语句
    /// </summary>
    [TestClass]
    public class QueryTest
    {
        BaseRepository<User> rep = new BaseRepository<User>();
        BaseRepository<User_Ext> rep1 = new BaseRepository<User_Ext>();

        [TestMethod]
        public void TestSelectCol()
        {
            var user = rep.Query().Select(u=> new User {
                 Name =u.Name,
                  Id = u.Id
            }).ToList();

            //SELECT
	        //`user`.`Name` AS `Name`,
	        //`user`.`Id` AS `Id`
            //FROM
	        //`user` AS `user`
        }


        [TestMethod]
        public void TestWhere()
        {
            var user = rep.Query().Where(u => u.Id == 1).FirstOrDefault();

            Assert.AreEqual(1, user.Id);
        }


        [TestMethod]
        public void TestLike()
        {
            var users = rep.Query().Where(u => u.Name.Contains("sw")).ToList();

            //SELECT
	        //`user`.`Id` AS `Id`,
	        //`user`.`Name` AS `Name`,
	        //`user`.`Gender` AS `Gender`,
	        //`user`.`Age` AS `Age`,
	        //`user`.`Status` AS `Status`,
	        //`user`.`Remark` AS `Remark`
            //FROM
	        //`user` AS `user`
            //WHERE
	        //`user`.`Name` LIKE CONCAT('%', N'sw', '%')

            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void TestIn()
        {
            List<int> ids = new List<int>() { 1, 2, 3 };
            var users = rep.Query().Where(u => ids.Contains(u.Id)).ToList();

            //SELECT
	        //`user`.`Id` AS `Id`,
	        //`user`.`Name` AS `Name`,
	        //`user`.`Gender` AS `Gender`,
	        //`user`.`Age` AS `Age`,
	        //`user`.`Status` AS `Status`,
	        //`user`.`Remark` AS `Remark`
            //FROM
            //`user` AS `user`
            //WHERE
            //`user`.`Id` IN(1, 2, 3)

            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void TestOrderBy()
        {
            var users = rep.Query().OrderBy(u => u.Name).ThenBy(u => u.Age).ToList();

            //SELECT
            //`user`.`Id` AS `Id`,
            //`user`.`Name` AS `Name`,
            //`user`.`Gender` AS `Gender`,
            //`user`.`Age` AS `Age`,
            //`user`.`Status` AS `Status`,
            //`user`.`Remark` AS `Remark`
            //FROM
            //`user` AS `user`
            //ORDER BY
            //`user`.`Name` ASC,
            //`user`.`Age` ASC
        }

        [TestMethod]
        public void TestPage()
        {
            var users = rep.Query().OrderBy(u => u.Age).TakePage(2, 3);

            //SELECT
	        //`user`.`Id` AS `Id`,
	        //`user`.`Name` AS `Name`,
	        //`user`.`Gender` AS `Gender`,
	        //`user`.`Age` AS `Age`,
	        //`user`.`Status` AS `Status`,
	        //`user`.`Remark` AS `Remark`
            //FROM
	        //`user` AS `user`
            //ORDER BY
	        //`user`.`Age` ASC
            //LIMIT 3,
            //3
        }

        [TestMethod]
        public void TestDistinct()
        {
            var users = rep.Query().Select(u => new { Name = u.Name}).Distinct();

            //SELECT DISTINCT `user`.`Name` AS `Name` FROM `user` AS `user`
        }

        [TestMethod]
        public void TestSubquery()
        {
            var users = rep.Query().Where(u => rep1.Query().Select(x => x.id).ToList().Contains(u.Id));

            //        SELECT
	        //`user`.`Id` AS `Id`,
	        //`user`.`Name` AS `Name`,
	        //`user`.`Gender` AS `Gender`,
	        //`user`.`Age` AS `Age`,
	        //`user`.`Status` AS `Status`,
	        //`user`.`Remark` AS `Remark`
            //FROM
	        //`user` AS `user`
            //WHERE
	        //`user`.`Id` IN(
            //    SELECT
	        //		`user_ext`.`id` AS `C`
            //
            //    FROM
	        //		`user_ext` AS `user_ext`
	        //)
        }
    }
}
