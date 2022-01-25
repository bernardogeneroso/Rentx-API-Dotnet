using Microsoft.AspNetCore.Mvc;
using Models;
using Services.CarsAppointments;

namespace API.Controllers;

[ApiController]
[Route("api/car/appointment")]
public class CarAppointmentController : BaseApiController
{

    [HttpPost("{plate}")]
    public async Task<IActionResult> CreateAppointment(string plate, [FromBody] CarAppointment carAppointment)
    {
        carAppointment.Plate = plate;

        return HandleResult(await Mediator.Send(new Create.Command { CarAppointment = carAppointment }));
    }
}
