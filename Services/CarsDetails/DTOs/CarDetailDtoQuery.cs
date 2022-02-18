using Services.CarsImages.DTOs;

namespace Services.CarsDetails.DTOs;

public class CarDetailDtoQuery
{
    public string Plate { get; set; }
    public int MaxSpeed { get; set; }
    public int TopSpeed { get; set; }
    public float Acceleration { get; set; }
    public int Weight { get; set; }
    public int Hp { get; set; }
    public CarImageDtoQuery[] Images { get; set; }
}
