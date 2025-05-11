using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Errors;

namespace Store.Es.APIs.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // to ignore these endpoints in the swagger
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error(int code)
        {
            return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Not found end point"));
        }
    }
}
