using PhilipYate.Application.Request;
using PhilipYate.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhilipYate.Application
{
    public class CreateOrderService
    {
        ProductRepository _productRep = new ProductRepository();
        ShopRepository _shopRep = new ShopRepository();

        public ReturnData Verify(List<CreateOrderReq> products)
        {
            var productIDs = products.Select(p => p.ProductID).ToList();
            var toBuyProducts = _productRep.GetProductsToBuy(productIDs);

            //判断商品信息，是否可卖
            foreach (var product in toBuyProducts)
            {
                if (!product.IsEnoughStock(l =>{
                        return l.Where(p => p.ProductID == product.ProductID)
                          .FirstOrDefault().buyCount;
                    }, products))
                {
                    return new ReturnData(500, product.ProductName + "库存不足");
                }

                if(!product.IsRightPrice())
                {
                    return new ReturnData(500, product.ProductName + "商品信息有误");
                }
            }

            //判断店铺信息，是否可交易
            var ownerIDs = toBuyProducts.Select(p => p.OwnerID).ToList();
            var shops = _shopRep.GetShopsByOwnerID(ownerIDs);
            foreach (var shop in shops)
            {
                if(!shop.Status)
                {
                    return new ReturnData(500, shop.ShopName + "店铺信息有误");
                }
            }

            return new ReturnData(200, "成功");
        }
    }
}
