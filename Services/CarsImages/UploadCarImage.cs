using Application.Core;
using AutoMapper;
using Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services.CarsImages;

public class UploadCarImage
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Plate { get; set; }
        public IFormFile File { get; set; }
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
            if (request.File.Length > 0)
            {
                // Manual validation using FluentValidation
                var validator = new UploadCarImageValidator();
                var resultValidation = await validator.ValidateAsync(request, cancellationToken);

                if (!resultValidation.IsValid) return Result<Unit>.Failure("Failed to upload image", resultValidation);

                var car = await _context.Cars.Include(i => i.Images).FirstOrDefaultAsync(x => x.Plate == request.Plate, cancellationToken);

                if (car == null) return Result<Unit>.Failure("Failed to upload image");

                var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";

                var path = await _imageAccessor.AddImage(request.File, fileName);

                if (path == null) return Result<Unit>.Failure("Failed to upload image");

                var currentMain = car.Images.FirstOrDefault(x => x.IsMain);

                var carImage = new CarImage
                {
                    Car = car,
                    ImageName = fileName,
                    IsMain = !(bool)currentMain?.IsMain
                };

                car.Images.Add(carImage);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to upload image");

                return Result<Unit>.SuccessNoContent(Unit.Value);
            }

            return Result<Unit>.Failure("Failed to upload image");
        }
    }
}
