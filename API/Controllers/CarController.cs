using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cars;
using Services.CarsDetails;

namespace API.Controllers;

public class CarController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCars([FromQuery] string search = null)
    {
        return HandleResult(await Mediator.Send(new List.Query { Search = search }));
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
    public async Task<IActionResult> GetCarDetails(string plate, [FromBody] CarDetailDto carDetailDto)
    {
        return HandleResult(await Mediator.Send(new Services.CarsDetails.Edit.Command { Plate = plate, CarDetailDto = carDetailDto }));
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

    [HttpPost("image/{plate}/setMain")]
    public async Task<IActionResult> SetMainImage(string plate, [FromQuery] string imageName)
    {
        return HandleResult(await Mediator.Send(new Services.CarsImages.SetMain.Command { Plate = plate, ImageName = imageName }));
    }

    [HttpGet("favorite")]
    public async Task<IActionResult> GetFavoriteCar()
    {
        return HandleResult(await Mediator.Send(new Services.CarsAppointments.FavoriteCar.Query()));
    }
}