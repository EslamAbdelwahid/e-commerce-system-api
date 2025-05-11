using Store.Es.Core.Dtos.CustomerBasket;
using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Services.Contract
{
    public interface IBasketService
    {
        Task<CustomerBasketDto?> GetBasketAsync(string id);
        Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
