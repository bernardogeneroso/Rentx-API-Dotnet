using FluentValidation;
using Services.CarsDetails.DTOs;

namespace Services.CarsDetails;

public class CarDetailValidator : AbstractValidator<CarDetailDtoRequest>
{
    public CarDetailValidator()
    {
        RuleFor(x => x.maxSpeed).GreaterThan(0).NotEmpty();
        RuleFor(x => x.topSpeed).GreaterThan(0).NotEmpty();
        RuleFor(x => x.acceleration).GreaterThan(0).NotEmpty();
        RuleFor(x => x.weight).GreaterThan(0).NotEmpty();
        RuleFor(x => x.hp).GreaterThan(0).NotEmpty();
    }
}
