using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Store.Es.Core.Dtos.Auth;
using Store.Es.Core.Entities.Identity;
using Store.Es.Core.Services.Contract;
using Store.Es.Repository.Extenstions;
using Store.Es.Service.Services.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        public async Task<AppUserDto> LogInAsync(LogInDto logIn)
        {
            var user = await _userManager.FindByEmailAsync(logIn.Email);
            if (user is null) return null;
            var result = await _signInManager.CheckPasswordSignInAsync(user, logIn.Password, false); // false here refer to I didn't want to lock any accounts
            if (!result.Succeeded) return null;
            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            AppUserDto appUserDto = new AppUserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token,

            };
            return appUserDto;
        }
        public async Task<AppUserDto> RegisterAsync(RegisterDto info)
        {
            if (await CheckEmailExistsAsync(info.Email)) return null;
            var appUser = new AppUser()
            {
                Email = info.Email,
                DisplayName = info.DisplayName,
                PhoneNumber = info.PhoneNumber,
                UserName = info.Email.Split("@")[0],

            };
            var addressDto = info.Address;
            if (addressDto is not null)
            {
                var address = _mapper.Map<Address>(addressDto);
                address.FirstName = info.DisplayName;
                appUser.Address = address;
            }

            var result = await _userManager.CreateAsync(appUser, info.Password);
            if (!result.Succeeded) return null;

            var appUserDto = new AppUserDto()
            {
                Email = info.Email,
                DisplayName = info.DisplayName,
                Token = await _tokenService.CreateTokenAsync(appUser, _userManager)
            };
            return appUserDto;
        }
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        public async Task<AppUserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return null;
            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            var userDto = new AppUserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token
            };
            return userDto;
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(ClaimsPrincipal claims)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(claims);
            
            if (user is null) return null;
            var addressDto = _mapper.Map<AddressDto>(user.Address);
            return addressDto;
        }

        public async Task<bool> UpdateCurrentUserAddressAsync(UpdateAddressDto addressDto, ClaimsPrincipal claims)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(claims);
            if (user is null) return false;

            if(user.Address is null)
            {
                user.Address = new Address()
                {
                    FirstName = user.DisplayName,
                    LastName = "Default"
                };
            }
            user.Address.Country = addressDto.Country;
            user.Address.City = addressDto.City;
            user.Address.Street = addressDto.Street;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
