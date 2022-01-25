using FluentValidation;

namespace Services.CarsAppointments;

public class CarAppointmentValidator : AbstractValidator<CarAppointmentDto>
{
    public CarAppointmentValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .Length(6);
        RuleFor(x => x.StartDate)
            .Must(BeValidDate)
            .WithMessage("Start date is required");
        RuleFor(x => x.EndDate).Must(BeValidDate).WithMessage("End date is required");
        RuleFor(x => x).Must(x => x.EndDate == default(DateTime) || x.StartDate == default(DateTime) || x.EndDate > x.StartDate)
            .WithMessage("End date must greater than start date");
    }

    private bool BeValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}
