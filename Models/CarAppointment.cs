namespace Models;

public class CarAppointment
{
    public Guid Id { get; set; }
    public string Plate { get; set; }
    public string UserId { get; set; }
    public Car Car { get; set; }
    public AppUser User { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public float RentalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
