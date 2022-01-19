using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services.CarsImage;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public CarImage CarImage { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CarImage).SetValidator(new CarImageValidator());
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
            if (await _context.CarsImage.AnyAsync(x => x.ImageName == request.CarImage.ImageName))
                return Result<Unit>.Failure("Car image already exists");

            // TODO: Create a new Provider for uploading images

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
