using Chloe;
using PhilipYate.Domin;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Repository
{
    public class ProductRepository: BaseRepository<Product>
    {
        public IList<Product> GetProductsToBuy(List<int> productIDs)
        {
            var products = Query().Where(p => productIDs.Contains(p.ProductID));

            return products.ToList();
        }
    }
}
