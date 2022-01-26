using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

            var query = _context.Cars
                    .Join(_context.CarsAppointments,
                                car => car.Plate,
                                appointment => appointment.Plate,
                                (car, appointment) => new { car, appointment })

                    .Where(x =>
                            (startDate > x.appointment.StartDate.Date || endDate < x.appointment.StartDate.Date)
                        && (startDate > x.appointment.EndDate.Date || endDate < x.appointment.EndDate.Date)
                        && request.Result.Fuel == x.car.Fuel
                        && request.Result.Transmission == x.car.Transmission
                    )
                    .Select(x => x.car)
                    .Distinct()
                    .ProjectTo<CarDto>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                    .AsQueryable();

            return Result<List<CarDto>>.Success(await query.ToListAsync());
        }
    }
}
