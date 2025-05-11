using AutoMapper;
using Store.Es.Core;
using Store.Es.Core.Dtos.Orders;
using Store.Es.Core.Entities;
using Store.Es.Core.Entities.Order;
using Store.Es.Core.Helper;
using Store.Es.Core.Services.Contract;
using Store.Es.Core.Specifications.Orders;
using Store.Es.Service.Services.CustomerBaskets;
using Store.Es.Service.Services.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderToReturnDto?> CreateOrderAsync(OrderDto orderDto, string userEmail)
        {
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId);
            if (basket is null) return null;
            var orderItems = new List<OrderItem>();
            if (basket.Items.Count() > 0)
            {
                foreach(var  item in basket.Items)
                {
                    var product =  await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    var productOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productOrder, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod is null) return null;
            decimal subTotal = orderItems.Sum(o => o.Price * o.Quantity);
            var shippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShipToAddress);

            if (basket.IntentPaymentId is not null)
            {
                // if there exists an Order with this IntentPaymentId we should remove it
                var orderWithPaymentSpec = new OrdersWithPaymentIntentSpec(basket.IntentPaymentId);
                var orderToDelete = await  _unitOfWork.Repository<Order, int>().GetWithSpecAsync(orderWithPaymentSpec);
                if (orderToDelete != null)
                {
                    // Ensure Items are loaded
                    if (orderToDelete.Items?.Any() == true)
                    {
                        //  Use repository to delete range (ensure it uses tracked entities)
                        _unitOfWork.Repository<OrderItem, int>().DeleteRange(orderToDelete.Items);

                    }

                    _unitOfWork.Repository<Order, int>().Delete(orderToDelete);
                }

            }

            var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basket.Id);

            var order = new Order(userEmail, orderItems, shippingAddress, deliveryMethod, subTotal, basketDto.IntentPaymentId);


            await _unitOfWork.Repository<Order, int>().AddAsync(order);   
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null; 
           
            return _mapper.Map<OrderToReturnDto>(order);    
        }

        public async Task<IEnumerable<DeliveryMethodDto>?> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (deliveryMethods is null) return null;
            return _mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<PaginationResponse<OrderToReturnDto>?> GetAllOrdersWithSpecAsync(OrderSpecParams orderParams)
        {
            var spec = new OrderSpecifications(orderParams);

            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);

            if (orders is null) return null;

            var ordersToReturnDto = _mapper.Map<IEnumerable<OrderToReturnDto>>(orders);

            var countSpec = new OrdersWithCountSpec(orderParams);

            int count = await _unitOfWork.Repository<Order, int>().GetCountAsync(countSpec);

            var response = new PaginationResponse<OrderToReturnDto>(orderParams.PageSize, orderParams.PageIndex, count, ordersToReturnDto);
            return response;

        }

        public async Task<OrderToReturnDto?> GetOrderWithSpecAsync(int orderId)
        {
            var spec = new OrderSpecifications(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (order is null) return null;
            return _mapper.Map<OrderToReturnDto>(order);
        }

    }
}
