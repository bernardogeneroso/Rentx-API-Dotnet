using Microsoft.AspNetCore.Identity;

namespace Models;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public string AvatarName { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<CarAppointment> Appointments { get; set; } = new List<CarAppointment>();
}

public enum Role
{
    User,
    Admin
}