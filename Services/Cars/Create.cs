using Application.Core;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

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
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _context.Cars.AnyAsync(x => x.Plate == request.Car.Plate))
                return Result<Unit>.Failure("Car already exists");

            await _context.Cars.AddAsync(request.Car);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to create car");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
