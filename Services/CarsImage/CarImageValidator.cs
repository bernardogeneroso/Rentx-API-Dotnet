using FluentValidation;
using Models;

namespace Services.CarsImage;

public class CarImageValidator : AbstractValidator<CarImage>
{
    public CarImageValidator()
    {
        RuleFor(x => x.Url).NotEmpty();
        RuleFor(x => x.ImageName).NotEmpty();
        RuleFor(x => x.IsMain).Must(x => x == false || x == true).NotEmpty();
    }
}
