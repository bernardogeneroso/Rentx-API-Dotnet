using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Providers.Security;

public class UserAccessor : IUserAccessor
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  public UserAccessor(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public string GetEmail()
  {
    return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
  }

  public string GetUsername()
  {
    return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
  }
}
