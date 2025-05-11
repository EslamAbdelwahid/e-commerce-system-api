using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Es.Core.Dtos.Products;
using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Store.Es.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration) 
        {
            CreateMap<Product, ProductDto>()
                .ForMember(p => p.BrandName, options => options.MapFrom(s => s.Brand.Name)) // as we can't map Brand to string (BrandName)
                .ForMember(p => p.TypeName, options => options.MapFrom(s => s.Type.Name))
                .ForMember(p => p.PictureUrl, options => options.MapFrom(s => $"{configuration["BASEURL"]}{s.PictureUrl}"));

            CreateMap<ProductType, TypeBrandDto>();
            CreateMap<ProductBrand, TypeBrandDto>();
        }
    }
}
