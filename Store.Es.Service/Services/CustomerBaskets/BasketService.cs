using AutoMapper;
using Store.Es.Core.Dtos.CustomerBasket;
using Store.Es.Core.Entities;
using Store.Es.Core.Repositories.Contract;
using Store.Es.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Service.Services.CustomerBaskets
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basket;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basket, IMapper mapper)
        {
            _basket = basket;
            _mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            bool exists = await _basket.DeleteBasketAsync(id);
            return exists;
        }

        public async Task<CustomerBasketDto?> GetBasketAsync(string id)
        {
            
            var basket = await _basket.GetBasketAsync(id);
            if (basket is null) return _mapper.Map<CustomerBasketDto>(new CustomerBasket() { Id = id });
            var mappedBasket = _mapper.Map<CustomerBasketDto>(basket);
            return mappedBasket;
        }

        public async Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto basket)
        {
           
            var mappedBasket = _mapper.Map<CustomerBasket> (basket);
            if (mappedBasket is null) return null;
            var result = await _basket.UpdateBasketAsync(mappedBasket);
            return basket;
        }
    }
}
