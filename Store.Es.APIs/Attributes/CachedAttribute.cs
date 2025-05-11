using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Es.Core.Services.Contract;
using System.Text;

namespace Store.Es.APIs.Attributes
{
    // I make it imp IAsyncActionFilter to check if the data in memory DB or not
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        { // this method will allow me to check if there exists a Data on Memory DB or not
            // this Delegete: ActionExecutionDelegate next -> when I invoke it will execute the endpoints which I flag it [Cache]
            
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();// ask clr to inject object from ICacheService

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var cachedResponse =  await cacheService.GetCacheKeyAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse)) // mean that response is cached will retrive it without back to DB
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next.Invoke(); // will call to the end-points which I write [Cach(int)] above it
            // now I need to cache the response
            if(executedContext.Result is OkObjectResult response) // result which will return from end-point
            {
                // here is OkObjectResult: to check if the response from type ok!
                await cacheService.SetCacheKeyAsync(cacheKey, response, TimeSpan.FromSeconds(_expireTime));
            }
        }
        private string GenerateCacheKey(HttpRequest request)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append(request.Path);
            foreach(var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }
            return cacheKey.ToString();
        }
    }
}
