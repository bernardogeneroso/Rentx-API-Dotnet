using FluentValidation;
using Services.CarsDetails.DTOs;

namespace Services.CarsDetails;

public class CarDetailValidator : AbstractValidator<CarDetailDtoRequest>
{
    public CarDetailValidator()
    {
        RuleFor(x => x.MaxSpeed).GreaterThan(0).NotEmpty();
        RuleFor(x => x.TopSpeed).GreaterThan(0).NotEmpty();
        RuleFor(x => x.Acceleration).GreaterThan(0).NotEmpty();
        RuleFor(x => x.Weight).GreaterThan(0).NotEmpty();
        RuleFor(x => x.Hp).GreaterThan(0).NotEmpty();
    }
}
