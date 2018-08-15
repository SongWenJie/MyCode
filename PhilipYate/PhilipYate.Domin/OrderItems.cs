using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Domin
{
    public class OrderItems
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }
  
    }
}
