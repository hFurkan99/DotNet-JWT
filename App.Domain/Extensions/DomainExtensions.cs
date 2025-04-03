using App.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainsExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CustomTokenOption>(options =>
                configuration.GetSection(CustomTokenOption.CustomToken).Bind(options));

            services.Configure<List<ClientOptions>>(options =>
                configuration.GetSection(ClientOptions.Clients).Bind(options));

            return services;
        }
    }
}
