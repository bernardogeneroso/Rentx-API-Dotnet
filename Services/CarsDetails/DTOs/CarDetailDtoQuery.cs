using Services.CarsImages.DTOs;

namespace Services.CarsDetails.DTOs;

public class CarDetailDtoQuery
{
    public string Plate { get; set; }
    public int maxSpeed { get; set; }
    public int topSpeed { get; set; }
    public float acceleration { get; set; }
    public int weight { get; set; }
    public int hp { get; set; }
    public CarImageDtoQuery[] Images { get; set; }
}
