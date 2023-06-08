using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using RedisCacheDemo.Configurations;
using RedisCacheDemo.Services;
using System.Text;

namespace RedisCacheDemo.Attributes
{
    public class Cache : Attribute, IActionFilter
    {
        public async void OnActionExecuting(ActionExecutingContext context)
        {
            var redisConfig = context.HttpContext.RequestServices.GetService<RedisConfiguration>();
            if (!redisConfig.Enabled) return;

            string cacheKey = GetCacheKey(context.HttpContext.Request);
            var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
            var cacheData = cacheService.Get(cacheKey).Result;
            if (!string.IsNullOrEmpty(cacheData))
            {
                context.Result = new OkObjectResult(cacheData);
            }
        }

        public async void OnActionExecuted(ActionExecutedContext context)
        {
            var redisConfig = context.HttpContext.RequestServices.GetService<RedisConfiguration>();
            if (!redisConfig.Enabled) return;

            if (context.Result is OkObjectResult objectResult)
            {
                string cacheKey = GetCacheKey(context.HttpContext.Request);
                var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
                await cacheService.Set(cacheKey, objectResult.Value, TimeSpan.FromDays(1));
            }
        }

        public static string GetCacheKey(HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(request.Path);
            foreach (KeyValuePair<string, StringValues> keyValuePair in request.Query.ToList())
            {
                sb.Append($"|{keyValuePair.Key}-{keyValuePair.Value}");
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString()));
        }
    }
}