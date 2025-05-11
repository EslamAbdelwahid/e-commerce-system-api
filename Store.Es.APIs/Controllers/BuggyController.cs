using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Errors;
using Store.Es.Repository.Data.Contexts;

namespace Store.Es.APIs.Controllers
{

    public class BuggyController : ApiBaseController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            this._context = context;
        }

        [HttpGet("notfound")]
        public async Task<IActionResult> GetNotFoundError()
        {
            var brand = await _context.Brands.FindAsync(100);
            if (brand is null) return NotFound(new ApiErrorResponse(404, "This Brand not found"));
            return Ok(brand);
        }
        [HttpGet("servererror")]
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _context.Brands.FindAsync(100);
            var brandToString = brand.ToString(); // Will throw exception
            return Ok(brand);
        }

        [HttpGet("badrequest")]
        public  async Task<IActionResult> GetBadRequestError()
        {
            return  BadRequest(new ApiErrorResponse(400));
        }
        [HttpGet("badrequest/{id}")]
        public async Task<IActionResult> GetBadRequestError(int id)
        {
            return  Ok();
        }
        [HttpGet("unauthorized")]
        public async Task<IActionResult> GetUnauthorizedError()
        {
            return Unauthorized(new ApiErrorResponse(401));
        }
    }
}
