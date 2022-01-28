using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cars;

namespace API.Controllers;

[ApiController]
[Route("api/car/appointment")]
public class CarAppointmentController : BaseApiController
{
    [HttpPost("{plate}")]
    public async Task<IActionResult> CreateAppointment(string plate, [FromBody] CarAppointment carAppointment)
    {
        carAppointment.Plate = plate;

        return HandleResult(await Mediator.Send(new Services.CarsAppointments.Create.Command { CarAppointment = carAppointment }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        return HandleResult(await Mediator.Send(new Services.CarsAppointments.Delete.Command { Id = id }));
    }

    [HttpGet("scheduled")]
    public async Task<IActionResult> GetUserScheduledCars()
    {
        return HandleResult(await Mediator.Send(new Services.CarsAppointments.UserScheduledCars.Query()));
    }

    [AllowAnonymous]
    [HttpGet("between-dates")]
    public async Task<IActionResult> GetCarsBetweenDates([FromQuery] CarsBetweenDatesResult carsBetweenDates)
    {
        return HandleResult(await Mediator.Send(new Services.Cars.CarsBetweenDates.Query { Result = carsBetweenDates }));
    }
}
