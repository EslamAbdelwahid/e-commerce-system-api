using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Errors;
using Store.Es.Core.Dtos.CustomerBasket;
using Store.Es.Core.Services.Contract;

namespace Store.Es.APIs.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService _basket;

        public BasketsController(IBasketService basket)
        {
            this._basket = basket;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket(string? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var basket = await _basket.GetBasketAsync(id);
            if(basket is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateBasket(CustomerBasketDto model)
        {
            var result = await _basket.UpdateBasketAsync(model);
            if (result is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(result);  
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasketAsync(string id)
        {
            bool flag = await _basket.DeleteBasketAsync(id);
            if (flag == false) return NotFound(new ApiErrorResponse(404));
            return Ok(flag);
        }
    }
}
