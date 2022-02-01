namespace Services.CarsDetails.DTOs;

public class CarDetailDtoRequest
{
    public int maxSpeed { get; set; }
    public int topSpeed { get; set; }
    public float acceleration { get; set; }
    public int weight { get; set; }
    public int hp { get; set; }
}
