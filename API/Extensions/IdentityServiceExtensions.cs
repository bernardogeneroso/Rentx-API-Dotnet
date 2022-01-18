using Database;
using Microsoft.AspNetCore.Identity;
using Models;

namespace API.Middleware;

public static class IdentityServiceExtensions
{
  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
  {
    // services.AddIdentityCore<AppUser>(opt =>
    // {
    //   opt.Password.RequireNonAlphanumeric = false;
    // }).AddEntityFrameworkStores<DataContext>().AddSignInManager<SignInManager<AppUser>>();

    return services;
  }
}
