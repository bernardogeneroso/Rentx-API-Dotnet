using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Models;

namespace Services.Cars;

public class Edit
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
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var car = await _context.Cars.FindAsync(request.Car.Plate);

            if (car == null) return null;

            request.Car.UpdatedAt = DateTime.UtcNow;

            _mapper.Map(request.Car, car);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to edit the car");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
