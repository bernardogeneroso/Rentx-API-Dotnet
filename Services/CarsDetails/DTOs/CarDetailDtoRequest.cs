namespace Services.CarsDetails.DTOs;

public class CarDetailDtoRequest
{
    public int MaxSpeed { get; set; }
    public int TopSpeed { get; set; }
    public float Acceleration { get; set; }
    public int Weight { get; set; }
    public int Hp { get; set; }
}
