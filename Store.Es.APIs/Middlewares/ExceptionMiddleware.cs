using Store.Es.APIs.Errors;
using System.Text.Json;

namespace Store.Es.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        // to call next middleware
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // Log the full exception including inner exceptions
                var innerExceptionMessage = ex.InnerException?.Message ?? "No inner exception";
                Console.WriteLine($"Exception: {ex.Message}, Inner: {innerExceptionMessage}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "An error occurred while processing your request.",
                    details = ex.ToString(), // This includes inner exceptions
                    statusCode = context.Response.StatusCode
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
