using Store.Es.Core.Dtos.Orders;
using Store.Es.Core.Entities.Order;
using Store.Es.Core.Helper;
using Store.Es.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<OrderToReturnDto?> CreateOrderAsync(OrderDto orderDto, string userEmail);
        Task<OrderToReturnDto?> GetOrderWithSpecAsync(int orderId);
        Task<PaginationResponse<OrderToReturnDto>?> GetAllOrdersWithSpecAsync(OrderSpecParams orderParams);
        Task<IEnumerable<DeliveryMethodDto>?> GetAllDeliveryMethodsAsync();


    }
}
