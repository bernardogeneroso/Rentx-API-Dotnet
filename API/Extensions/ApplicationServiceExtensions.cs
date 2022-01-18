using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Providers.Security;
using Services.Interfaces;

namespace API.Middleware;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    });
    services.AddDbContext<DataContext>(opt =>
    {
      opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    });
    services.AddCors(opt =>
    {
      opt.AddPolicy("CorsPolicy", policy =>
      {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
      });
    });

    services.AddScoped<IUserAccessor, UserAccessor>();

    return services;
  }
}
