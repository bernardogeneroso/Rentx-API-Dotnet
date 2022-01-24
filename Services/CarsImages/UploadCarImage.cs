using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services.CarsImages;

public class UploadCarImage
{
    public class Command : IRequest<Result<CarImageDto>>
    {
        public string Plate { get; set; }
        public IFormFile File { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Plate).Length(6).NotEmpty();
            RuleFor(x => x.File.Length).NotNull().LessThanOrEqualTo(100)
                .WithMessage("File size is larger than allowed");
            RuleFor(x => x.File.ContentType).Must(x => x.Contains("image")).WithMessage("File must be an image").NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<CarImageDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IImageAccessor _imageAccessor;
        public Handler(DataContext context, IWebHostEnvironment environment, IImageAccessor imageAccessor, IMapper mapper)
        {
            _imageAccessor = imageAccessor;
            _environment = environment;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<CarImageDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.File.Length > 0)
            {
                // TODO: Verify path of the image

                var car = await _context.Cars.Include(i => i.CarImages).FirstOrDefaultAsync(x => x.Plate == request.Plate);

                if (car == null) return Result<CarImageDto>.Failure("Failed to upload image");

                var fileName = $"{Guid.NewGuid().ToString()}_{request.File.FileName}";

                var path = await _imageAccessor.AddImage(request.File, fileName);

                if (path == null) return Result<CarImageDto>.Failure("Failed to upload image");

                var currentMain = car.CarImages.FirstOrDefault(x => x.IsMain);

                var carImage = new CarImage
                {
                    Car = car,
                    ImageName = fileName,
                    IsMain = currentMain?.IsMain == true ? false : true
                };

                await _context.CarsImages.AddAsync(carImage);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<CarImageDto>.Failure("Failed to upload image");

                var carImageDto = _mapper.Map<CarImageDto>(carImage);

                carImageDto.Url = path;

                return Result<CarImageDto>.Success(carImageDto);
            }

            return Result<CarImageDto>.Failure("Failed to upload image");
        }
    }
}
