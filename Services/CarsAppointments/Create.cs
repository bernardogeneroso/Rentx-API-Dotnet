using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.CarsAppointments.DTOs;
using Services.Interfaces;

namespace Services.CarsAppointments;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Plate { get; set; }
        public CarAppointmentDtoRequest CarAppointment { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Plate).Length(6).NotEmpty();
            RuleFor(x => x.CarAppointment).SetValidator(new CarAppointmentValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<Unit>.Failure("Faield creating car appointment");

            var car = await _context.Cars.FindAsync(request.Plate);

            if (car == null) return Result<Unit>.Failure("Faield creating car appointment");

            var startDate = request.CarAppointment.StartDate.Date;
            var endDate = request.CarAppointment.EndDate.Date;

            var userHasMoreCarAppointments = await _context.CarsAppointments.AnyAsync(x => x.UserId == user.Id
                    && ((x.StartDate.Date <= startDate && x.EndDate.Date >= startDate)
                    || (x.EndDate.Date >= endDate && x.StartDate.Date <= endDate)));

            if (userHasMoreCarAppointments) return Result<Unit>.Failure("The user only can have one car appointment per date");

            var existCarAppointmentsBetweenDates = await _context.CarsAppointments
                .AnyAsync(x => x.Plate == car.Plate && ((x.StartDate.Date <= startDate && x.EndDate.Date >= startDate)
                    || (x.EndDate.Date >= endDate && x.StartDate.Date <= endDate)));

            if (existCarAppointmentsBetweenDates) return Result<Unit>.Failure("This car is already reserved for this period");

            var days = (endDate - startDate).Days + 1;

            var appointment = new CarAppointment
            {
                Car = car,
                User = user,
                StartDate = startDate,
                EndDate = endDate,
                RentalPrice = car.PricePerDay * days
            };

            car.Appointments.Add(appointment);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Faield creating car appointment");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
