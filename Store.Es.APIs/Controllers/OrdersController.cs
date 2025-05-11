using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Errors;
using Store.Es.Core.Dtos.Orders;
using Store.Es.Core.Entities.Order;
using Store.Es.Core.Helper;
using Store.Es.Core.Services.Contract;
using Store.Es.Core.Specifications.Orders;
using System.Security.Claims;

namespace Store.Es.APIs.Controllers
{
    // BaseUrl/api/Orders
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
           // return Ok(true);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var orderToReturnDto = await _orderService.CreateOrderAsync(orderDto, userEmail);

            if (orderToReturnDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(orderToReturnDto);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrdersForUser(OrderSpecParams specParams)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var paginationResponse = await _orderService.GetAllOrdersWithSpecAsync(specParams);

            if (paginationResponse is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(paginationResponse);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderByIdForUser(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var orderToReturnDto = await _orderService.GetOrderWithSpecAsync(id);

            if (orderToReturnDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(orderToReturnDto);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var response = await _orderService.GetAllDeliveryMethodsAsync();
            if (response is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(response);
        }
    }
}
