using FluentValidation;
using Services.CarsAppointments.DTOs;

namespace Services.CarsAppointments;

public class CarAppointmentValidator : AbstractValidator<CarAppointmentDtoRequest>
{
    public CarAppointmentValidator()
    {
        RuleFor(x => x.StartDate)
            .Must(BeValidDate)
            .WithMessage("Start date it must be valid and required");
        RuleFor(x => x.EndDate)
            .Must(BeValidDate)
            .WithMessage("End date it must be valid and required");
        RuleFor(x => x).Must(x => x.EndDate == default(DateTime) || x.StartDate == default(DateTime) || x.EndDate > x.StartDate)
            .WithMessage("End date must greater than start date");
    }

    private bool BeValidDate(DateTime date)
    {
        if (date.Equals(default(DateTime))) return false;

        return date > DateTime.Now;
    }
}
