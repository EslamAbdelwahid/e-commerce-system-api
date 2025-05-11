using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new();
        public int? DeliveryMethodId { get; set; }
        public string? IntentPaymentId { get; set; }
        public string? ClientSecret { get; set; }
       
    }
}
