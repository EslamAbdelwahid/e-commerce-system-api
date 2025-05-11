using Store.Es.Core.Dtos.Baskets;
using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Dtos.CustomerBasket
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? IntentPaymentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
