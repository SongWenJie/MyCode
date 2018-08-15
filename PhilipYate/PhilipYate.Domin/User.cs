using Chloe.Annotations;
using System;

namespace PhilipYate.Domin
{
    [Table("user")]
    public class User
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}
