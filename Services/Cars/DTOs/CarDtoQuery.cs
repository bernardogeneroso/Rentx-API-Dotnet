using Services.CarsImages.DTOs;

namespace Services.Cars.DTOs;

public class CarDtoQuery
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public CarImageDtoQuery Image { get; set; }
}
