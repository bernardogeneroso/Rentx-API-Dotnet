using Microsoft.AspNetCore.Identity;

namespace Models;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public string AvatarName { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<CarAppointment> CarAppointments { get; set; }
}

public enum Role
{
    User,
    Admin
}