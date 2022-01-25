using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services.CarsAppointments;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public CarAppointment CarAppointment { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
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

            var car = await _context.Cars.FindAsync(request.CarAppointment.Plate);

            if (car == null) return Result<Unit>.Failure("Faield creating car appointment");

            var startDate = request.CarAppointment.StartDate.Date;
            var endDate = request.CarAppointment.EndDate.Date;

            var existCarAppointmentsBetweenDates = await _context.CarsAppointments.AnyAsync(x => x.Plate == car.Plate && x.StartDate >= startDate && x.EndDate <= endDate);

            if (existCarAppointmentsBetweenDates) return Result<Unit>.Failure("This appointment already exist");

            var days = (endDate - startDate).Days;

            request.CarAppointment.RentalPrice = car.PricePerDay * days;

            request.CarAppointment.User = user;
            request.CarAppointment.Car = car;
            request.CarAppointment.StartDate = startDate;
            request.CarAppointment.EndDate = endDate;

            await _context.CarsAppointments.AddAsync(request.CarAppointment);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Faield creating car appointment");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
