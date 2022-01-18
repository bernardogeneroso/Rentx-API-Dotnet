using Microsoft.AspNetCore.Identity;
using Models;

namespace Database;

public class Seed
{
  public static async Task SeedData(UserManager<AppUser> userManager)
  {
    if (!userManager.Users.Any())
    {
      var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "bob",
                    Email = "bob@test.com",
                    Role = Role.User
                },
                new AppUser
                {
                    UserName = "jim",
                    Email = "jim@test.com",
                    Role = Role.User
                },
                new AppUser
                {
                    UserName = "admin",
                    Email = "admin@test.com",
                    Role = Role.Admin
                }
            };

      foreach (var user in users)
      {
        await userManager.CreateAsync(user, "Pa$$w0rd");
      }
    }
  }
}
