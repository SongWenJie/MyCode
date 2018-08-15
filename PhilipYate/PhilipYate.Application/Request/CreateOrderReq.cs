using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Application.Request
{
    public class CreateOrderReq
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int buyCount { get; set; }
    }
}
