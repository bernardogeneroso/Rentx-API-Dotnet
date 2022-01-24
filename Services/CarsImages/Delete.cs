using Application.Core;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.CarsImages;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Plate { get; set; }
        public string ImageName { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Plate).Length(6).NotNull();
            RuleFor(x => x.ImageName).NotEmpty();
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
            var image = await _context.CarsImages.FindAsync(request.Plate, request.ImageName);

            if (image == null) return Result<Unit>.Failure("Failed to delete the image");

            var resultImage = _imageAccessor.DeleteImage(image.ImageName);

            if (!resultImage) return Result<Unit>.Failure("Failed to delete the image");

            if (image.IsMain)
            {
                var firstImage = await _context.CarsImages.FirstOrDefaultAsync(x => x.Plate == image.Plate);

                if (firstImage != null)
                {
                    firstImage.IsMain = true;
                }
            }

            _context.CarsImages.Remove(image);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete the image");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
