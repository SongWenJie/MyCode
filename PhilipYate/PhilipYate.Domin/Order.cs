using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Domin
{
    [Table("order")]
    public class Order
    {
        /// <summary>
        /// 订单主键
        /// </summary>
        public int OrderID{ get; set; }

        /// <summary>
        /// 购买者主键
        /// </summary>
        public int BuyerID { get; set; }

        /// <summary>
        /// 购买者名字
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime Buytime { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

    }

    public enum OrderStatus
    {
        /// <summary>
        /// 待付款
        /// </summary>
        PendingPayment = 1,

        /// <summary>
        /// 待发货
        /// </summary>
        AwaitDeliver = 2,

        /// <summary>
        /// 待收货
        /// </summary>
        AwaitReceipt = 3,

        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 5,

        /// <summary>
        /// 已退货
        /// </summary>
        Returned =6 
    }
}
