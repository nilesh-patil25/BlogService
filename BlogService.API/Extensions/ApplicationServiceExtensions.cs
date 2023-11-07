using BlogService.Core.Services.Interfaces;
using BlogService.Core.Services;
using BlogService.DataAccess.Respositories.Interfaces;
using BlogService.DataAccess.Respositories;
using BlogService.DataAccess;
using Microsoft.EntityFrameworkCore;
using BlogService.DataAccess.DbInitializer;

namespace BlogService.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<BlogPostDbContext>(options => options.UseSqlServer(
            config.GetConnectionString("BlogServiceDB")
            ));

            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}
