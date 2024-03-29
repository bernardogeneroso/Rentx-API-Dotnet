namespace Models;

public class Car : BaseEntity
{
    public string Plate { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string Year { get; set; }
    public string Fuel { get; set; }
    public string Transmission { get; set; }
    public string Doors { get; set; }
    public string Seats { get; set; }
    public float PricePerDay { get; set; }
    public ICollection<CarImage> Images { get; set; } = new List<CarImage>();
    public ICollection<CarAppointment> Appointments { get; set; } = new List<CarAppointment>();
    public CarDetail Detail { get; set; }
}