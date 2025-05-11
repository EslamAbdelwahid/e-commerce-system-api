using Store.Es.Core.Dtos.Auth;
using Store.Es.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Services.Contract
{
    public interface IUserService
    {
        Task<AppUserDto> LogInAsync(LogInDto logIn);
        Task<AppUserDto> RegisterAsync(RegisterDto info);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<AppUserDto> GetCurrentUserAsync(string email);
        Task<AddressDto> GetCurrentUserAddressAsync(ClaimsPrincipal claims);
        Task<bool> UpdateCurrentUserAddressAsync(UpdateAddressDto addressDto, ClaimsPrincipal claims);

    }
}
