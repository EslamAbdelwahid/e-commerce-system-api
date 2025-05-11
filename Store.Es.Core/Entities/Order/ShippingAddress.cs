using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Entities.Order
{
    public class ShippingAddress
    {
        // didn't have here Id as this entity didn't store in DB as table (this is part of Order)
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

    }

}
