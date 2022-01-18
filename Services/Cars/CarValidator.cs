using FluentValidation;
using Models;

namespace Services.Cars;

public class CarValidator : AbstractValidator<Car>
{
    public CarValidator()
    {
        RuleFor(x => x.Plate).Length(6).NotEmpty();
        RuleFor(x => x.Brand).NotEmpty();
        RuleFor(x => x.Model).NotEmpty();
        RuleFor(x => x.Color).NotEmpty();
        RuleFor(x => x.Year).NotEmpty();
        RuleFor(x => x.Fuel).Must(BeValidFuel).WithMessage("Fuel should be either 'Gasoline', 'Diesel', 'Hybrid' or 'Electric'").NotEmpty();
        RuleFor(x => x.Transmission).Must(BeValidTransmission).WithMessage("Transmission should be either 'Manual' or 'Automatic'").NotEmpty();
        RuleFor(x => x.Doors).NotEmpty();
        RuleFor(x => x.Seats).NotEmpty();
        RuleFor(x => x.PricePerDay).NotEmpty();
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
