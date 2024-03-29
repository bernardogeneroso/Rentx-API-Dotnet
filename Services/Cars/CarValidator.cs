using FluentValidation;
using Services.Cars.DTOs;
using Services.CarsDetails;

namespace Services.Cars;

public class CarValidator : AbstractValidator<CarDtoRequest>
{
    public CarValidator()
    {
        RuleFor(x => x.Brand).NotEmpty();
        RuleFor(x => x.Model).NotEmpty();
        RuleFor(x => x.Color).NotEmpty();
        RuleFor(x => x.Year).NotEmpty();
        RuleFor(x => x.Fuel).Must(BeValidFuel).WithMessage("Fuel should be either 'Gasoline', 'Diesel', 'Hybrid' or 'Electric'").NotEmpty();
        RuleFor(x => x.Transmission).Must(BeValidTransmission).WithMessage("Transmission should be either 'Manual' or 'Automatic'").NotEmpty();
        RuleFor(x => x.Doors).NotEmpty();
        RuleFor(x => x.Seats).NotEmpty();
        RuleFor(x => x.PricePerDay).NotEmpty();
        RuleFor(x => x.Detail).SetValidator(new CarDetailValidator()).NotEmpty();
    }

    private bool BeValidFuel(string fuel)
    {
        return fuel == "Gasoline" || fuel == "Diesel" || fuel == "Hybrid" || fuel == "Electric";
    }

    private bool BeValidTransmission(string transmission)
    {
        return transmission == "Manual" || transmission == "Automatic";
    }
}
