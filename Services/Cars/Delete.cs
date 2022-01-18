using Application.Core;
using Database;
using MediatR;

namespace Services.Cars;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Plate { get; set; }
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
            var car = _context.Cars.Find(request.Plate);

            if (car == null) return null;

            _context.Cars.Remove(car);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete the car");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
