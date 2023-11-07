using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BlogService.API.Middlewares
{
    public class CustomClientRateLimitMiddleware : ClientRateLimitMiddleware
    {
        public CustomClientRateLimitMiddleware(RequestDelegate next, IProcessingStrategy processingStrategy, IOptions<ClientRateLimitOptions> options, IClientPolicyStore policyStore, IRateLimitConfiguration config, ILogger<ClientRateLimitMiddleware> logger) : base(next, processingStrategy, options, policyStore, config, logger)
        {
        }

        public override Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
        {

            var result = JsonSerializer.Serialize("API calls Quota Exceeded!");
            httpContext.Response.Headers["Retry-After"] = retryAfter;
            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            httpContext.Response.ContentType = "application/json";

            return httpContext.Response.WriteAsync(result);
        }
    }
}
