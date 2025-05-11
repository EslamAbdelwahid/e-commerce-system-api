using Microsoft.AspNetCore.Identity;
using Store.Es.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Repository
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> userManager)
        {
            if(userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    UserName = "islaam_abdelwahid",
                    Email = "eslamabdelwahid9@gmail.com",
                    DisplayName = "Eslam Abdelwahid",
                    PhoneNumber = "01014584363",
                    Address = new Address()
                    {
                        Country = "Egypt",
                        City = "Menouf",
                        Street = "Susoft",
                        FirstName = "Eslam",
                        LastName = "Abdelwahid"
                    }
                };
                await userManager.CreateAsync(user, "000000Aa##@@");
            }
        }
    }
}
