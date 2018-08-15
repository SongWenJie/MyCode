using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhilipYate.Application;
using PhilipYate.Application.Request;
using System.Collections.Generic;

namespace PhilipYate.ApplicationTest
{
    [TestClass]
    public class UnitTest1
    {
        CreateOrderService service = new CreateOrderService();
        [TestMethod]
        public void TestMethod1()
        {
            var result = service.Verify(new List<CreateOrderReq> {
                new CreateOrderReq {  ProductID =1, buyCount =2},
                new CreateOrderReq {  ProductID =2 , buyCount = 10}
            });
        }
    }
}
