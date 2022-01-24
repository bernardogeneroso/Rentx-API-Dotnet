using Microsoft.AspNetCore.Identity;

namespace Models;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
    public string AvatarName { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum Role
{
    User,
    Admin
}