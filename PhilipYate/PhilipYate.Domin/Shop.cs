using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Domin
{
    [Table("shop")]
    public class Shop
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 状态 0正常，1作废
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 店主id(对应user表主键)
        /// </summary>
        public int OwnerID { get; set; }

    }
}
