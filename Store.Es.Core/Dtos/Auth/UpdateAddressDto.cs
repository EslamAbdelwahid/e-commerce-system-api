using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Dtos.Auth
{
    public class UpdateAddressDto
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
    }
}
