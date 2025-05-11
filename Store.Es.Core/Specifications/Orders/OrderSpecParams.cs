using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Specifications.Orders
{
    public class OrderSpecParams
    {
        public string? Sort {  get; set; }
        public int? DeliveryMethodId { get; set; }

        public int PageSize { get; set; } = 2;
        public int PageIndex { get; set; } = 1;
    }
}
