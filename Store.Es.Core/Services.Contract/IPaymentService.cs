using Store.Es.Core.Dtos.CustomerBasket;
using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Services.Contract
{
    public interface IPaymentService
    {
       Task<CustomerBasketDto?> CreateOrUpdatePaymentIntentIdAsync(string basketId);
    }
}
