using Microsoft.Extensions.Configuration;
using Store.Es.Core;
using Store.Es.Core.Dtos.CustomerBasket;
using Store.Es.Core.Entities;
using Store.Es.Core.Entities.Order;
using Store.Es.Core.Services.Contract;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Es.Core.Entities.Product;

namespace Store.Es.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;

            // Validate Stripe configuration is available
            if (string.IsNullOrEmpty(_configuration["Stripe:SecretKey"]))
            {
                throw new InvalidOperationException(
                    "Stripe Secret Key is not configured. Please ensure you've set up user secrets correctly.");
            }

            // Set Stripe API key once during service initialization
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<CustomerBasketDto?> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            // No need to set StripeConfiguration.ApiKey here anymore
            // It's now set in the constructor

            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            decimal shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                if (item.Price != product.Price)
                {
                    item.Price = product.Price;
                }
            }

            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.IntentPaymentId))
            {
                // Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await service.CreateAsync(options);
                basket.IntentPaymentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100)
                    // the price only may change as the customer my add/remove items 
                };
                paymentIntent = await service.UpdateAsync(basket.IntentPaymentId, options);
                basket.IntentPaymentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            // now update the basket
            var result = await _basketService.UpdateBasketAsync(basket);
            return result;
        }
    }
}