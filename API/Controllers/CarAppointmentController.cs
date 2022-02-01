using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Cars.DTOs;
using Services.CarsAppointments.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/car/appointment")]
public class CarAppointmentController : BaseApiController
{
    [HttpPost("{plate}")]
    public async Task<IActionResult> CreateAppointment(string plate, [FromBody] CarAppointmentDtoRequest carAppointment)
    {
        return HandleResult(await Mediator.Send(new Services.CarsAppointments.Create.Command { Plate = plate, CarAppointment = carAppointment }));
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
    public async Task<IActionResult> GetCarsBetweenDates([FromQuery] CarsBetweenDatesDtoRequest carsBetweenDates)
    {
        return HandleResult(await Mediator.Send(new Services.Cars.CarsBetweenDates.Query { Result = carsBetweenDates }));
    }
}
