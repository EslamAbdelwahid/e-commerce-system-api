using Store.Es.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Dtos.Orders
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public string Status { get; set; } // this was enum and stored as a string in DB mapping can handle this

        public ShippingAddressDto ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; } // we should make configurations for this in profile
        public decimal DeliveryMethodCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
