using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Es.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Repository.Extenstions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal claims)
        {
            var email = claims.FindFirstValue(ClaimTypes.Email);
            if (email == null) { return null; }
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
