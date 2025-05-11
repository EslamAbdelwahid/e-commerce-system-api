using Store.Es.Core.Entities;
using Store.Es.Core.Entities.Order;
using Store.Es.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Es.Repository
{
    public static class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext _context)
        {
            if(_context.Brands.Count() == 0)
            {
                // 1. Read Data from json file
                var brandsData = File.ReadAllText(@"..\Store.Es.Repository\Data\DataSeed\brands.json");
                // 2. Convert json string to List<T>
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                // Seed Data to DB
                if (brands is not null && brands.Count > 0)
                {
                    await _context.Brands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
            }

            if(_context.Types.Count() == 0)
            {
                var typesData = File.ReadAllText(@"..\Store.Es.Repository\Data\DataSeed\types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if(types is not null && types.Count > 0)
                {
                    await _context.Types.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.Products.Count() == 0)
            {
                var productsType = File.ReadAllText(@"..\Store.Es.Repository\Data\DataSeed\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsType);
                if (products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }

            if(_context.DeliveryMethods.Count() == 0)
            {
                var file = File.ReadAllText(@"..\Store.Es.Repository\Data\DataSeed\delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(file);  
                if(deliveryMethods is not null && deliveryMethods.Count > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();
                }
            }



        }
    }
}
