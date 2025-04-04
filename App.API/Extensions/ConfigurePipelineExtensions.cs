using Microsoft.Extensions.Options;

namespace App.API.Extensions
{
    public static class ConfigurePipelineExtensions
    {
        public static IApplicationBuilder UseConfigurePipelineExt(this WebApplication app)
        {
            app.UseExceptionHandlerExt();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt();
            }

            app.UseHttpsRedirection();

            app.UseCors(option =>
            {
                option.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:3000");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
