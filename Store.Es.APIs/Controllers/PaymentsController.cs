using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Errors;
using Store.Es.Core.Services.Contract;

namespace Store.Es.APIs.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var paymentIntent = await paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);
            if(paymentIntent is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(paymentIntent);
        }
    }
}
