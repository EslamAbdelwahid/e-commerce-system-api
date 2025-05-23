﻿using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string id);
        Task<bool> DeleteBasketAsync(string id);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
    }
}
