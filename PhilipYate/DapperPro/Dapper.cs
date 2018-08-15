using MySql.Data.MySqlClient;
using System;
using Dapper;
using static Dapper.SqlMapper;
using System.Linq;
using System.Collections.Generic;

namespace DapperDemo
{
    public class Dapper
    {
        static readonly string conStr = "Server=192.168.11.128;port=3339;Database=sakila;Uid=root;Pwd=123456;";

        public static int Execute(string sql ,object param)
        {
            using (var connection = new MySqlConnection(conStr))
            {                           
                int count  = connection.Execute(sql, param);
                return count;
            }
        }

        public static IEnumerable<T> Query<T>(string sql, object param)
        {
            using (var connection = new MySqlConnection(conStr))
            {
                var result = connection.Query<T>(sql, param);
                return result;
            }
        }

        /// <summary>
        /// QueryFirst
        /// No Item : Exception
        /// One Item : Item
        /// Many Items : First Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QueryFirst<T>(string sql, object param)
        {
            using (var connection = new MySqlConnection(conStr))
            {
                var result = connection.QueryFirst<T>(sql, param);
                return result;
            }
        }

        /// <summary>
        /// QuerySingle
        /// No Item : Exception
        /// One Item : Item
        /// Many Items : Exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数</param>
        /// <returns></returns>
        public static T QuerySingle<T>(string sql, object param)
        {
            using (var connection = new MySqlConnection(conStr))
            {
                var result = connection.QuerySingle<T>(sql, param);
                return result;
            }
        }

        /// <summary>
        /// QueryFirstOrDefault
        /// No Item : Default
        /// One Item : Item
        /// Many Items : First Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数</param>
        /// <returns></returns>
        public static T QueryFirstOrDefault<T>(string sql, object param)
        {
            using (var connection = new MySqlConnection(conStr))
            {
                var result = connection.QueryFirstOrDefault<T>(sql, param);
                return result;
            }
        }

        /// <summary>
        /// QuerySingleOrDefault
        /// No Item : Default
        /// One Item : Item
        /// Many Items : Exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数</param>
        /// <returns></returns>
        public static T QuerySingleOrDefault<T>(string sql, object param)
        {
            using (var connection = new MySqlConnection(conStr))
            {
                var result = connection.QuerySingleOrDefault<T>(sql, param);
                return result;
            }
        }

        //只能在方法内部使用的原因是方法执行完using会释放资源，链接已经断掉
        //可以使用下面的方法，在客户端使用using释放资源
        //public static GridReader QueryMultiple(string sql, object param)
        //{
        //    using (var connection = new MySqlConnection(conStr))
        //    {
        //        //connection.Open();

        //        //var result = connection.QueryMultiple(sql, param);

        //        using (var mul = connection.QueryMultiple(sql, param))
        //        {
        //            var users = mul.Read<User>().ToList();
        //            var actors = mul.Read<Actor>().ToList();
        //        }
        //        return null;
        //    }
        //}

        public static GridReader QueryMultiple(string sql, object param)
        {
            var connection = new MySqlConnection(conStr);           
            var mul = connection.QueryMultiple(sql, param);

            return mul;
        }


        public static IEnumerable<TReturn> Query<TFirst,TSecond,TReturn>(
            string sql, Func<TFirst, TSecond, TReturn> map, object param, string splitOn = "Id")
        {
            using (var connection = new MySqlConnection(conStr))
            {
                var result = connection.Query<TFirst, TSecond, TReturn>(sql, map, param, splitOn: splitOn);
                return result;
            }
        }
    }

}
