using AutoMapper;
using Store.Es.Core.Dtos.Auth;
using Store.Es.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Mapping.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile() 
        {
            CreateMap<AddressDto, Address>()
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(x => "Default"));
            CreateMap<Address, AddressDto>();
        }
    }
}
