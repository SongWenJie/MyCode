using PhilipYate.Domin;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Repository
{
    public class ShopRepository : BaseRepository<Shop>
    {
        public IList<Shop> GetShopsByOwnerID(List<int> ownerID)
        {
            var shops = Query().Where(s=>ownerID.Contains(s.OwnerID))
                .Select(s => new Shop
                {
                    ID = s.ID,
                    ShopName = s.ShopName,
                    Status = s.Status
                });

            return shops.ToList();
        }
    }
}
