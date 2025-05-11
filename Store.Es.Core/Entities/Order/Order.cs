using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Entities.Order
{
    public class Order : BaseEntity<int>
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, ICollection<OrderItem> items, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            Items = items;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow; // Date of Order Creation

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost; // not mapped in Db this is (Method)
        public string? PaymentIntentId { get; set; }
    }
}
