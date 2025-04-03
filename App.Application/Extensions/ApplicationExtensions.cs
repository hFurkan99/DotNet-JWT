using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace App.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddServicesExt(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);


            //services.AddScoped<IProductService, ProductService>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
