using Store.Es.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Specifications.Orders
{
    public class OrderSpecifications : BaseSpecifications<Order, int>
    {
        public OrderSpecifications(int orderId) : base(o => o.Id == orderId)
        {
            ApplyIncludes();
        }
        public OrderSpecifications(OrderSpecParams orderParams) : base(
            o => 
            (!orderParams.DeliveryMethodId.HasValue || orderParams.DeliveryMethodId == o.DeliveryMethodId)
            )
        {
            if (string.IsNullOrEmpty(orderParams.Sort))
            {
                AddOrderBy(o => o.OrderDate);
            }
            else
            {
                switch(orderParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(o => o.GetTotal());
                            break;
                    case "priceDesc":
                        AddOrderByDesc(o => o.GetTotal());
                            break;
                    default:
                        AddOrderBy(o => o.OrderDate);
                        break;
                }
            }
            ApplyIncludes();
            int skip = (orderParams.PageIndex - 1) * orderParams.PageSize;
            ApplyPagination(skip, orderParams.PageSize);
        }
        private void ApplyIncludes()
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
