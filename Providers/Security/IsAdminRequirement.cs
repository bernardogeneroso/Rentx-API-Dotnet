using System.Security.Claims;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Providers.Security;

public class IsAdminRequirement : IAuthorizationRequirement
{
}

public class IsAdminRequirementHandler : AuthorizationHandler<IsAdminRequirement>
{
    private readonly DataContext _dbContext;
    public IsAdminRequirementHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
    {
        var userEmail = context.User.FindFirstValue(ClaimTypes.Email);

        if (userEmail == null) return Task.CompletedTask;

        var user = _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Email == userEmail).Result;

        if (user == null) return Task.CompletedTask;

        if (user.Role == Role.Admin) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
