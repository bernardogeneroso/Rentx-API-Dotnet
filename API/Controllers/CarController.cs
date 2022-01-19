using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cars;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCars()
    {
        return HandleResult(await Mediator.Send(new List.Query()));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar([FromBody] Car car)
    {
        return HandleResult(await Mediator.Send(new Create.Command { Car = car }));
    }

    [HttpPut("{plate}")]
    public async Task<IActionResult> EditCar(string plate, [FromBody] Car car)
    {
        car.Plate = plate;

        return HandleResult(await Mediator.Send(new Edit.Command { Car = car }));
    }

    [HttpDelete("{plate}")]
    public async Task<IActionResult> DeleteCar(string plate)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Plate = plate }));
    }
}
