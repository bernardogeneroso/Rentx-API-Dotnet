using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Cars.DTOs;
using Services.Interfaces;

namespace Services.Cars;

public class CarsBetweenDates
{
    public class Query : IRequest<Result<List<CarDtoQuery>>>
    {
        public CarsBetweenDatesDtoRequest Result { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(x => x.Result).SetValidator(new CarsBetweenDatesValidator());
        }
    }

    public class Handler : IRequestHandler<Query, Result<List<CarDtoQuery>>>
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

        public async Task<Result<List<CarDtoQuery>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var startDate = request.Result.StartDate.Date;
            var endDate = request.Result.EndDate.Date;

            var carsWithAppointmentsBetweenDates = await _context.CarsAppointments
                            .AsNoTracking()
                            .Where(x => (x.StartDate.Date <= startDate && x.EndDate.Date >= startDate)
                                    || (x.EndDate.Date >= endDate && x.StartDate.Date <= endDate))
                            .Select(x => x.Plate)
                            .Distinct()
                            .ToListAsync(cancellationToken);

            if (carsWithAppointmentsBetweenDates == null) return Result<List<CarDtoQuery>>.Failure("Failed getting cars between dates");

            var cars = await _context.Cars
                            .AsNoTracking()
                            .Where(x => x.Fuel == request.Result.Fuel
                            && x.Transmission == request.Result.Transmission
                            && x.PricePerDay >= request.Result.StartPricePerDay
                            && x.PricePerDay <= request.Result.EndPricePerDay)
                            .Where(x => !carsWithAppointmentsBetweenDates.Contains(x.Plate))
                            .ProjectTo<CarDtoQuery>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                            .ToListAsync(cancellationToken);

            return Result<List<CarDtoQuery>>.Success(cars);
        }
    }
}
