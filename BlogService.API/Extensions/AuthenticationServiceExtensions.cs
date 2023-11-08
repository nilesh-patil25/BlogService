using BlogService.Core;
using BlogService.Core.AppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogService.API.Extensions
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddJWTAuthenticationServices(this IServiceCollection services)
        {
            var secretKey = AppSettingsHelper.GetValue(ConfigConstants.JWTSecretKey);

            var key = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = AppSettingsHelper.GetValue(ConfigConstants.JWTIssuer),
                    ValidAudience = AppSettingsHelper.GetValue(ConfigConstants.JWTAudience)
                };
            });

            return services;
        }
    }
}
