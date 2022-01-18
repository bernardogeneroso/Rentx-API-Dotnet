using Microsoft.AspNetCore.Identity;

namespace Models;

public class AppUser : IdentityUser
{
  public string AvatarUrl { get; set; }
  public string AvatarName { get; set; }
  public Role Role { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

}

public enum Role
{
  User,
  Admin
}