using Store.Es.Core.Dtos.Products;
using Store.Es.Core.Entities;
using Store.Es.Core.Helper;
using Store.Es.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Services.Contract
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams specParams);
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);

    }
}
