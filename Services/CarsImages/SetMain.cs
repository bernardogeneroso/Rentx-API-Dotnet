using Application.Core;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.CarsImages;

public class SetMain
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Plate { get; set; }
        public string ImageName { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Plate).Length(6).NotEmpty();
                RuleFor(x => x.ImageName).NotEmpty();
            }
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var images = await _context.CarsImages.Where(x => x.Plate == request.Plate).ToListAsync();

            var image = images.FirstOrDefault(x => x.ImageName == request.ImageName);

            if (image == null) return Result<Unit>.Failure("Failed to set main image");

            var oldImage = images.FirstOrDefault(x => x.IsMain);

            if (oldImage == null) return Result<Unit>.Failure("Failed to set main image");

            image.IsMain = true;
            oldImage.IsMain = false;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to set main image");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
