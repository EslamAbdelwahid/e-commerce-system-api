using Store.Es.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Specifications.Orders
{
    public class OrdersWithCountSpec : BaseSpecifications<Order, int>
    {
        public OrdersWithCountSpec(OrderSpecParams orderParams) : base(
            o =>
            (!orderParams.DeliveryMethodId.HasValue || orderParams.DeliveryMethodId == o.DeliveryMethodId)
            )
        {
            
        }
    }
}
