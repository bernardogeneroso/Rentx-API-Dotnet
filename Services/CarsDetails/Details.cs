using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.CarsDetails;

public class Details
{
    public class Query : IRequest<Result<CarDetailDto>>
    {
        public string Plate { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(x => x.Plate).Length(6).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Query, Result<CarDetailDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IOriginAccessor _originAccessor;
        public Handler(DataContext context, IMapper mapper, IOriginAccessor originAccessor)
        {
            _originAccessor = originAccessor;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<CarDetailDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var carDetail = await _context.CarsDetails
                    .Include(x => x.Car.CarImages)
                    .Where(x => x.Car.Plate == request.Plate)
                    .ProjectTo<CarDetailDto>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                    .SingleOrDefaultAsync(x => x.Plate == request.Plate);

            if (carDetail == null) return Result<CarDetailDto>.Failure("Failed to get the car details");

            return Result<CarDetailDto>.Success(carDetail);
        }
    }
}
