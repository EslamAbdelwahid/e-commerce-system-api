using Store.Es.APIs.Middlewares;
using Store.Es.Repository.Data.Contexts;
using Store.Es.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Store.Es.Core.Entities.Identity;
using Store.Es.Repository.Identity.Contexts;

namespace Store.Es.APIs.Helper
{
    public static class ConfigureMiddlewares
    {
        public static async Task<WebApplication> AddMiddlewaresAsync(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>(); // Configure user-defined middleware

            #region To Update Database
            // after Build and before Run
            using var scope = app.Services.CreateScope(); // Contains all services from Scope type like (StoreDbContext)
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreDbContext>();
            var identityContext = services.GetRequiredService<StoreIdentityDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            var userManager = services.GetRequiredService < UserManager < AppUser >> ();
            // Defenced Code
            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);

                await identityContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message, "Threre are some issues during apply migrations");
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // here {0} refer to first parameter in Error endpoiont which is {code}
            app.UseStatusCodePagesWithRedirects("/error/{0}"); // to handle not found end points

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            return app;
        }
    }
}
