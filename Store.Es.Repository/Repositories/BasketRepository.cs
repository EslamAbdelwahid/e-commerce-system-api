using StackExchange.Redis;
using Store.Es.Core.Entities;
using Store.Es.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Es.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket) 
        {
            var json = JsonSerializer.Serialize(basket);
            bool addOrUpdateBasket = await _database.StringSetAsync(basket.Id, json, TimeSpan.FromSeconds(120));
            if (addOrUpdateBasket == false) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
