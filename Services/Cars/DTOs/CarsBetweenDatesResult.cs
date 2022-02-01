namespace Services.Cars.DTOs;

public class CarsBetweenDatesDtoRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public float StartPricePerDay { get; set; }
    public float EndPricePerDay { get; set; }
    public string Fuel { get; set; }
    public string Transmission { get; set; }
}
