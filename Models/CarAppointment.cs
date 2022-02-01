namespace Models;

public class CarAppointment : BaseEntity
{
    public Guid Id { get; set; }
    public string Plate { get; set; }
    public string UserId { get; set; }
    public Car Car { get; set; }
    public AppUser User { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public float RentalPrice { get; set; }
}
