using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Es.Core.Entities.Identity;
using Store.Es.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Service.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"];

            var roles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };
            
            foreach(var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            //SymmetricSecurityKey : Represents the secret key used to sign the token.
            //It is created by encoding the secretKey string into bytes using UTF-8 encoding.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            //SigningCredentials : Defines how the token will be signed.
            //In this case, it uses the EcdsaSha256Signature algorithm (SecurityAlgorithms.EcdsaSha256Signature) for signing.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                 issuer: jwtSettings["Issuer"],
                 audience: jwtSettings["Audience"],
                 expires: DateTime.Now.AddDays(double.Parse(jwtSettings["DurationInDays"])),
                 claims: authClaims,
                 signingCredentials: credentials
                );
        // JwtSecurityTokenHandler : A utility class for working with JWTs.
        // WriteToken: Converts the JwtSecurityToken object into a string representation(the actual JWT).
        // The returned string is the final JWT, which can be sent to the client for authentication.
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
