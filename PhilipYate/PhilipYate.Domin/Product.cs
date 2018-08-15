using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Domin
{
    [Table("product")]
    public class Product
    {
        /// <summary>
        /// 商品主键
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 库存量
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 拥有者ID
        /// </summary>
        public int OwnerID { get; set; }


        /// <summary>
        /// 判断商品库存是否充足
        /// </summary>
        /// <param name="buyCount">购买数量</param>
        /// <returns></returns>
        public bool IsEnoughStock<T>(Func<T,int> buyCountFunc,T product)
        {
            return Stock >= buyCountFunc(product);
        }

        /// <summary>
        /// 判断销售价是否不低于成本价
        /// </summary>
        /// <returns></returns>
        public bool IsRightPrice()
        {
            return SellPrice >= CostPrice;
        }
    }
}
