using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Es.Core.Dtos.Orders;
using Store.Es.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Mapping.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile(IConfiguration configuration)
        {
            

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethodCost, options => options.MapFrom(src => src.DeliveryMethod.Cost))
                .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName));

            CreateMap<ShippingAddressDto, ShippingAddress>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => $"{configuration["BASEURL"]}{src.Product.PictureUrl}"));

        }
    }
}
