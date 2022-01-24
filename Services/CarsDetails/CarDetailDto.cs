namespace Services.CarsDetails;

public class CarDetailDto
{
    public int maxSpeed { get; set; }
    public int topSpeed { get; set; }
    public float acceleration { get; set; }
    public int weight { get; set; }
    public int hp { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
