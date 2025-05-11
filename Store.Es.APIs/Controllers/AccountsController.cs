using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Errors;
using Store.Es.Core.Dtos.Auth;
using Store.Es.Core.Services.Contract;
using Store.Es.Service.Services.Users;
using System.Security.Claims;
using static Azure.Core.HttpHeader;

namespace Store.Es.APIs.Controllers
{
    public class AccountsController : ApiBaseController
    {
        private readonly IUserService _userService;
        public AccountsController(IUserService userService) 
        {
            _userService = userService;
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AppUserDto>> LogIn(LogInDto info)
        {
            var userDto = await _userService.LogInAsync(info);
            if(userDto is null) return  Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return  Ok(userDto);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AppUserDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AppUserDto>> Register(RegisterDto info)
        {
            var userDto = await _userService.RegisterAsync(info);
            if (userDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalild SignUp"));
            return Ok(userDto);
        }
        [Authorize]
        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<AppUserDto>> GetCurrentUser()
        {
            #region User email null
            //You're accessing the current user's identity, which comes from:

            //    1. Either cookies(for web apps)
            //    2. Or a JWT bearer token(in your case)
            //    If there's no token , then User.Identity.IsAuthenticated == false, and all claim values (like email) will be null or missing.               
            //    This doesn't cause an error — just gives you a null email, and thus your service can't find the user.
            #endregion
            var userEmail =  User.FindFirstValue(ClaimTypes.Email); // this need token to give you current user email
            var userDto = await _userService.GetCurrentUserAsync(userEmail);
            if (userDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalild SignUp"));
            return Ok(userDto);
        }

        [Authorize]
        [HttpGet("getaddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {; 
            var addressDto = await _userService.GetCurrentUserAddressAsync(User);
            if(addressDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "This address is null"));
            return Ok(addressDto);
        }
        [Authorize]
        [HttpPut("updateaddress")]
        public async Task<ActionResult<UpdateAddressDto>> UpdateCurrentUserAddress(UpdateAddressDto addressDto)
        {
            var result = await _userService.UpdateCurrentUserAddressAsync(addressDto, User);
            if(!result) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "This address is null"));
            return Ok(addressDto);
        }
    }
}
