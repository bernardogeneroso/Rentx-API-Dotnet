using Services.Cars.DTOs;

namespace Services.CarsAppointments.DTOs;

public class FavoriteCarDtoQuery : CarDtoQuery
{
    public int TotalRentalDays { get; set; }
}
