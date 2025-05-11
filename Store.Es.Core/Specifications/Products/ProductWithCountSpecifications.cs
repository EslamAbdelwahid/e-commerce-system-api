using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Specifications.Products
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecParams specParams) : base
            (p =>
            (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search))
            &&
            (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId)
            &&
            (!specParams.TypeId.HasValue || p.TypeId == specParams.TypeId)
            )
        {
            
        }
    }
}
