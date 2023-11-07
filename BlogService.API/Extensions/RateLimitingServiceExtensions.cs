using AspNetCoreRateLimit;

namespace BlogService.API.Extensions
{
    public static class RateLimitingServiceExtensions
    {
        public static IServiceCollection AddRateLimitServices(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddInMemoryRateLimiting();

            services.Configure<ClientRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = StatusCodes.Status429TooManyRequests;
                options.ClientIdHeader = "Client-Id";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "10s",
                        Limit = 2
                    }
                };
            });

            return services;
        }
    }
}
