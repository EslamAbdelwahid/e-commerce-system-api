using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Specifications.Products
{
    public class ProductSpecifications : BaseSpecifications<Product, int>
    {


        public ProductSpecifications(int id) : base(p => p.Id == id)
        {
            ApplyIncludes();
        }
        public ProductSpecifications(ProductSpecParams specParams) : base
            (
            p =>
            (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search))
            &&
            (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId)
            && 
            (!specParams.TypeId.HasValue || p.TypeId == specParams.TypeId)
            ) 
        {
            if (string.IsNullOrEmpty(specParams.Sort))
            {
                AddOrderBy(p => p.Name);
            }
            else
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            ApplyIncludes();
            // 200 products
            // pageSize = 50, pageIndex = 2
            // skip = (pageIndex - 1) * pageSize
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }

        private void ApplyIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }
    }
}
