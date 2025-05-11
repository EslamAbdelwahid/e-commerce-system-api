using Store.Es.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Specifications.Orders
{
    public class OrdersWithPaymentIntentSpec : BaseSpecifications<Order, int>
    {
        public OrdersWithPaymentIntentSpec(string paymentIntentId):base(o => o.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(o => o.Items);
        }
    }
}
