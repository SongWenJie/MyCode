using Chloe;
using Chloe.MySql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CholeDemo
{
    /// <summary>
    /// 几个细节需要注意一下
    /// 1.BaseRepository<T>为什么泛型作用范围要限定在类级别上？
    /// 因为这样在业务层可以明确知道操作的是哪个实体类（表），越不灵活，也就越稳定，越容易维护
    /// 2.IQuery<T> Query()为什么返回 context.Query<T>()，而不是直接返回context上下文？
    /// 因为直接返回上下文就破坏了 BaseRepository<T>类的封装性，
    /// 导致业务层可以通过context上下文直接完成增删改等操作，
    /// 而 IQuery<T> Query()方法仅仅是提供查询操作，并且在业务层using释放资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T>
    {
        static readonly string connString = "Server=192.168.11.128;port=3339;Database=sakila;Uid=root;Pwd=123456;";
        //public MySqlContext context = new MySqlContext(new MySqlConnectionFactory(connString));

        private MySqlContext CreateContext()
        {
            MySqlContext context = new MySqlContext(new MySqlConnectionFactory(connString));
            return context;
        }

        public IQuery<T> Query()
        {
            var context = CreateContext();
            var result = context.Query<T>();
            return result;
        }

        public T Insert(T model)
        {
            using (var context = CreateContext())
            {
                var result = context.Insert<T>(model);
                return result;
            }
        }

        public bool Insert1(T model)
        {
            using (var context = CreateContext())
            {
                var result = context.Insert<T>(model);
                return result != null;
            }
        }

        public bool Insert1(Expression<Func<T>> content)
        {
            using (var context = CreateContext())
            {
                var result = context.Insert<T>(content);
                return result != null;
            }
        }

        public object Insert2(Expression<Func<T>> content)
        {
            using (var context = CreateContext())
            {
                var id = context.Insert<T>(content);
                return id;
            }
        }

        /// <summary>
        /// 更新方法  宋文杰 2018年8月9日14:01:47
        /// 只支持单条记录更新并且更新整条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">要更新的实体</param>
        /// <returns></returns>
        public bool Update(T model)
        {
            using (var context = CreateContext())
            {
                int result = context.Update<T>(model);
                return result > 0;
            }
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
        public bool Update(Expression<Func<T, bool>> condition, Expression<Func<T, T>> content)
        {
            using (var context = CreateContext())
            {
                int result = context.Update<T>(condition, content);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除方法 宋文杰 2018年8月9日14:13:24
        /// 只支持删除单条记录
        /// 并且实体必须指定主键（根据主键删除）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(T model)
        {
            using (var context = CreateContext())
            {
                int result = context.Delete<T>(model);
                return result > 0;
            }
        }


        /// <summary>
        /// 删除方法 宋文杰 2018年8月9日14:31:29
        /// 支持批量删除（指定删除条件）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">更新条件</param>
        /// <param name="content">更新内容</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> condition)
        {
            using (var context = CreateContext())
            {
                int result = context.Delete<T>(condition);
                return result > 0;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
