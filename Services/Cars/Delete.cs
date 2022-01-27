using Application.Core;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Cars;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Plate { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Plate).Length(6).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IImageAccessor _imageAccessor;
        public Handler(DataContext context, IImageAccessor imageAccessor)
        {
            _imageAccessor = imageAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var car = await _context.Cars.Include(x => x.CarImages).FirstOrDefaultAsync(x => x.Plate == request.Plate);

            if (car == null) return null;

            if (car.CarImages.Any())
            {
                foreach (var image in car.CarImages)
                {
                    _imageAccessor.DeleteImage(image.ImageName);
                }
            }

            _context.Cars.Remove(car);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete the car");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
