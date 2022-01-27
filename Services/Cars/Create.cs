using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.SignalR.Hubs;
using Services.SignalR.Interfaces;

namespace Services.Cars;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public Car Car { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Car).SetValidator(new CarValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper, IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            _mapper = mapper;
            _hubContext = hubContext;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var carExist = await _context.Cars.AnyAsync(x => x.Plate == request.Car.Plate);

            if (carExist) return Result<Unit>.Failure("Car already exists");

            _context.Cars.Add(request.Car);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to create car");

            await _hubContext.Clients.All.BroadcastMessage($"New car has been created | {request.Car.Brand} {request.Car.Model}");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
