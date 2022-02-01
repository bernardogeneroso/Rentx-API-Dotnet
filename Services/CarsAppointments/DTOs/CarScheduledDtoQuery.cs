using Services.Cars.DTOs;

namespace Services.CarsAppointments.DTOs;

public class CarScheduledDtoQuery : CarDtoQuery
{
    public CarAppointmentDtoQuery Appointment { get; set; }
}