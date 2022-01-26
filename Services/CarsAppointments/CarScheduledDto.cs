using Services.Cars;

namespace Services.CarsAppointments;

public class CarScheduledDto : CarDto
{
    public CarAppointmentDto Appointment { get; set; }
}
