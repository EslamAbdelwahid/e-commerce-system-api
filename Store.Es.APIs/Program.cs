
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Es.APIs.Errors;
using Store.Es.APIs.Helper;
using Store.Es.APIs.Middlewares;
using Store.Es.Core;
using Store.Es.Core.Mapping.Products;
using Store.Es.Core.Services.Contract;
using Store.Es.Repository;
using Store.Es.Repository.Data.Contexts;
using Store.Es.Service.Services.Products;

namespace Store.Es.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();

            await app.AddMiddlewaresAsync();

            app.Run();
        }
    }
}
