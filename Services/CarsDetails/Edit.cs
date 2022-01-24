using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Models;

namespace Services.CarsDetails;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Plate { get; set; }
        public CarDetail CarDetail { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Plate).Length(6).NotEmpty();
            RuleFor(x => x.CarDetail).NotEmpty();
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
            var carDetails = await _context.CarsDetails.FindAsync(request.Plate);

            if (carDetails == null) return Result<Unit>.Failure("Failed to edit the car details");

            carDetails.UpdatedAt = DateTime.UtcNow;

            _mapper.Map(request.CarDetail, carDetails);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to edit the car details");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
