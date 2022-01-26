using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
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

    public class Handler : IRequestHandler<Command, Result<CarImageDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageAccessor _imageAccessor;
        public Handler(DataContext context, IImageAccessor imageAccessor, IOriginAccessor originAccessor, IMapper mapper)
        {
            _imageAccessor = imageAccessor;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<CarImageDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.File.Length > 0)
            {
                // Manual validation using FluentValidation
                var validator = new UploadCarImageValidator();
                var resultValidation = await validator.ValidateAsync(request, cancellationToken);

                if (!resultValidation.IsValid) return Result<CarImageDto>.Failure("Failed to upload image", resultValidation);

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
