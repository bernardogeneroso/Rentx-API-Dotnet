using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Models;
using Services.Interfaces;

namespace Services.Cars;

public class CarsBetweenDates
{
    public class Query : IRequest<Result<List<CarDto>>>
    {
        public CarsBetweenDatesResult Result { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(x => x.Result).SetValidator(new CarsBetweenDatesValidator());
        }
    }

    public class Handler : IRequestHandler<Query, Result<List<CarDto>>>
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

        public async Task<Result<List<CarDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var startDate = request.Result.StartDate.Date;
            var endDate = request.Result.EndDate.Date;

            var carsWithAppointmentsBetweenDates = await _context.CarsAppointments
                            .Where(x => (x.StartDate.Date <= startDate && x.EndDate.Date >= startDate)
                                    || (x.EndDate.Date >= endDate && x.StartDate.Date <= endDate))
                            .Select(x => x.Plate)
                            .Distinct()
                            .ToListAsync();

            if (carsWithAppointmentsBetweenDates == null) return Result<List<CarDto>>.Failure("Faield getting cars between dates");

            var cars = await _context.Cars
                            .Where(x => x.Fuel == request.Result.Fuel
                            && x.Transmission == request.Result.Transmission
                            && x.PricePerDay >= request.Result.StartPricePerDay
                            && x.PricePerDay <= request.Result.EndPricePerDay)
                            .Where(x => !carsWithAppointmentsBetweenDates.Contains(x.Plate))
                            .ProjectTo<CarDto>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                            .ToListAsync();

            return Result<List<CarDto>>.Success(cars);
        }
    }
}
