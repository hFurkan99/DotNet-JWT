using App.API.Extensions;
using App.Application.Extensions;
using App.Domain.Extensions;
using App.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllersWithFiltersExt()
    .AddSwaggerGenExt()
    .AddExceptionHandlerExt()
    .AddRepositoriesExt(builder.Configuration)
    .AddServicesExt()
    .AddDomainsExt(builder.Configuration);

var app = builder.Build();

app.UseConfigurePipelineExt();

app.MapControllers();

app.Run();
