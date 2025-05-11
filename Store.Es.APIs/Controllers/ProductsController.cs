using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Attributes;
using Store.Es.APIs.Errors;
using Store.Es.Core.Dtos.Products;
using Store.Es.Core.Helper;
using Store.Es.Core.Services.Contract;
using Store.Es.Core.Specifications.Products;

namespace Store.Es.APIs.Controllers
{

    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }
        [Authorize]
        [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)] // will improve swagger docs
        [HttpGet] // Get BaseUrl/api/Products
        [Cached(90)] // Cache the response for 90 seconds
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams specParams)
        {
            var result = await _productService.GetAllProductsAsync(specParams);
            return Ok(result);
        }
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Product Id"));
            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Not found Product"));
            return Ok(product);

        }
        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>) ,StatusCodes.Status200OK)]
        [HttpGet("brands")] // Get BaseUrl/api/Products/brands 
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }
        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")] // Get BaseUrl/api/Products/types 
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }
    }
}
