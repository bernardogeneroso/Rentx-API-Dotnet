using FluentValidation;
using Services.Cars.DTOs;

namespace Services.Cars;

public class CarsBetweenDatesValidator : AbstractValidator<CarsBetweenDatesDtoRequest>
{
    public CarsBetweenDatesValidator()
    {
        RuleFor(x => x.StartDate)
            .Must(BeValidDate)
            .WithMessage("Start date it must be valid")
            .NotEmpty()
            .WithMessage("Start date is required");
        RuleFor(x => x.EndDate)
            .Must(BeValidDate)
            .WithMessage("End date it must be valid")
            .NotEmpty()
            .WithMessage("End date is required");
        RuleFor(x => x.StartPricePerDay)
            .Must(BeValidPricePerDay)
            .WithMessage("Start price per day it must be valid")
            .NotEmpty()
            .WithMessage("Start price per day is required");
        RuleFor(x => x.EndPricePerDay)
            .Must(BeValidPricePerDay)
            .WithMessage("End price per day it must be valid")
            .NotEmpty()
            .WithMessage("End price per day is required");
        RuleFor(x => x.Fuel)
            .Must(BeValidFuel)
            .WithMessage("Fuel should be either 'Gasoline', 'Diesel', 'Hybrid' or 'Electric'")
            .NotEmpty()
            .WithMessage("Fuel is required");
        RuleFor(x => x.Transmission)
            .Must(BeValidTransmission)
            .WithMessage("Transmission should be either 'Manual' or 'Automatic'")
            .NotEmpty()
            .WithMessage("Transmission is required");
    }

    private bool BeValidDate(DateTime date)
    {
        if (date.Equals(default(DateTime))) return false;

        return date > DateTime.Now;
    }

    private bool BeValidPricePerDay(float pricePerDay)
    {
        return pricePerDay > 0;
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
