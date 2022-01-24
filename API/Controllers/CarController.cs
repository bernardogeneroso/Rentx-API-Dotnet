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

        return HandleResult(await Mediator.Send(new Services.Cars.Edit.Command { Car = car }));
    }

    [HttpDelete("{plate}")]
    public async Task<IActionResult> DeleteCar(string plate)
    {
        return HandleResult(await Mediator.Send(new Services.Cars.Delete.Command { Plate = plate }));
    }

    [HttpGet("details/{plate}")]
    public async Task<IActionResult> GetCarDetails(string plate)
    {
        return HandleResult(await Mediator.Send(new Services.CarsDetails.Details.Query { Plate = plate }));
    }

    [HttpPut("details/{plate}")]
    public async Task<IActionResult> GetCarDetails(string plate, [FromBody] CarDetail carDetail)
    {
        return HandleResult(await Mediator.Send(new Services.CarsDetails.Edit.Command { Plate = plate, CarDetail = carDetail }));
    }

    [HttpPost("image/{plate}")]
    public async Task<IActionResult> UploadImage(string plate, [FromForm] IFormFile File)
    {
        return HandleResult(await Mediator.Send(new Services.CarsImages.UploadCarImage.Command { Plate = plate, File = File }));
    }

    [HttpDelete("image/{plate}")]
    public async Task<IActionResult> DelelteImage(string plate, [FromQuery] string imageName)
    {
        return HandleResult(await Mediator.Send(new Services.CarsImages.Delete.Command { Plate = plate, ImageName = imageName }));
    }
}