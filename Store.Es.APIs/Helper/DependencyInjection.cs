using Microsoft.EntityFrameworkCore;
using Store.Es.Core.Services.Contract;
using Store.Es.Core;
using Store.Es.Repository;
using Store.Es.Repository.Data.Contexts;
using Store.Es.Service.Services.Products;
using Store.Es.Core.Mapping.Products;
using Microsoft.AspNetCore.Mvc;
using Store.Es.APIs.Errors;
using Store.Es.Service.Services.CustomerBaskets;
using Store.Es.Core.Repositories.Contract;
using Store.Es.Repository.Repositories;
using StackExchange.Redis;
using Store.Es.Core.Mapping.Baskets;
using Store.Es.Service.Services.Caches;
using Store.Es.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Es.Service.Services.Users;
using Store.Es.Service.Services.Tokens;
using Store.Es.Repository.Identity.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Es.Core.Mapping.Auth;
using Store.Es.Core.Mapping.Orders;
using Store.Es.Service.Services.Orders;
using Store.Es.Service.Services.Payments;

namespace Store.Es.APIs.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddUserDefinedService();
            services.AddAutoMapperService(configuration);
            services.ConfigureInvalidModelStateResponseService();
            services.AddRedisService(configuration);
            services.AddIdentityService();
            services.AddAuthenticationService(configuration);
            return services;
        }
        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();
            return services;
        }
        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });
            return services;
        }
        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }

        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));
            services.AddAutoMapper(m => m.AddProfile(new AuthProfile()));
            services.AddAutoMapper(m => m.AddProfile(new OrderProfile(configuration)));
            return services;
        }
        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            // to do validation error response instead of default response

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // InvalidModelStateResponseFactory -> is responsible for any error about ModelState
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(e => e.ErrorMessage)
                                                         .ToArray();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
        private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            return services;
        }
        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>(); // all DI for all Identity built-in services
            return services;
        }
        /// <summary>
        /// Extension method to add authentication services to the service collection.
        /// This configures JWT Bearer authentication in an ASP.NET Core application.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Application configuration, used to read JWT settings from appsettings.json.</param>
        /// <returns>The updated IServiceCollection.</returns>
        private static IServiceCollection AddAuthenticationService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure the default authentication schemes for the application.
            // Here we're using JWT Bearer tokens for both authentication and challenge responses.
            services.AddAuthentication(options =>
            {
                // Default scheme used to authenticate the user (i.e., validate the token).
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // Scheme used when authentication fails and we need to challenge the user (e.g., return 401).
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Add JWT bearer authentication to the pipeline.
            .AddJwtBearer(options =>
            {
                // Configure how incoming JWT tokens are validated.
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    // Ensure the token has a valid issuer.
                    ValidateIssuer = true,

                    // The expected issuer of the token (e.g., your API or identity server).
                    ValidIssuer = configuration["Jwt:Issuer"],

                    // Ensure the token is intended for a specific audience (optional but recommended).
                    ValidateAudience = true,

                    // The expected audience of the token (e.g., "web-app" or "mobile-app").
                    ValidAudience = configuration["Jwt:Audience"],

                    // Make sure the token hasn't expired.
                    ValidateLifetime = true,

                    // Ensure the token was signed by a trusted key.
                    ValidateIssuerSigningKey = true,

                    // The actual signing key used to verify the token's signature.
                    // It should match the one used to generate the token.
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])
                    )
                };
            });

            // Return the modified service collection for fluent chaining.
            return services;
        }
    }
}
