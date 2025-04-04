using App.API.Filters;
using App.Application.Helpers;
using App.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IServiceCollection AddControllersWithFiltersExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                var tokenOptions = configuration.GetSection(CustomTokenOption.CustomToken).Get<CustomTokenOption>();

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions!.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignHelper.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                };
            });

            services.AddControllers(options =>
            {
                options.Filters.Add<FluentValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            services.AddCors();

            return services;    
        }
    }
}
