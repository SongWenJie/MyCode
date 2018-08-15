using Chloe.MySql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CholeDemo
{
    public class UnitWork
    {
        static readonly string connString = "Server=192.168.11.128;port=3339;Database=sakila;Uid=root;Pwd=123456;";
       
        MySqlContext context = new MySqlContext(new MySqlConnectionFactory(connString));

        public void BeginTransaction()
        {
            context.Session.BeginTransaction();
        }

        public void CommitTransaction()
        {
            using (context)
            {
                try
                {
                    context.Session.CommitTransaction();
                }
                catch
                {
                    if (context.Session.IsInTransaction)
                        context.Session.RollbackTransaction();
                    throw;
                }
            }
        }


        public void Insert<T>(T model)
        {
            var result = context.Insert<T>(model);
        }

        /// <summary>
        /// 更新方法  宋文杰 2018年8月9日14:01:47
        /// 只支持单条记录更新并且更新整条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">要更新的实体</param>
        /// <returns></returns>
        public void Update<T>(T model)
        {
            context.Update<T>(model);
        }

        /// <summary>
        /// 更新方法 宋文杰 2018年8月9日14:04:01
        /// 支持批量更新（指定更新条件）
        /// 支持更新部分列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">更新条件</param>
        /// <param name="content">更新内容</param>
        /// <returns></returns>
        public void Update<T>(Expression<Func<T, bool>> condition, Expression<Func<T, T>> content)
        {
            context.Update<T>(condition, content);
        }

        /// <summary>
        /// 删除方法 宋文杰 2018年8月9日14:13:24
        /// 只支持删除单条记录
        /// 并且实体必须指定主键（根据主键删除）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Delete<T>(T model)
        {
             context.Delete<T>(model);
        }


        /// <summary>
        /// 删除方法 宋文杰 2018年8月9日14:31:29
        /// 支持批量删除（指定删除条件）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">更新条件</param>
        /// <param name="content">更新内容</param>
        /// <returns></returns>
        public void Delete<T>(Expression<Func<T, bool>> condition)
        {
            context.Delete<T>(condition);
        }
    }
}
