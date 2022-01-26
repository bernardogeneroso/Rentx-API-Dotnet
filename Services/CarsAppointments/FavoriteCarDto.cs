using Services.Cars;

namespace Services.CarsAppointments;

public class FavoriteCarDto : CarDto
{
    public int TotalRentalDays { get; set; }
}
