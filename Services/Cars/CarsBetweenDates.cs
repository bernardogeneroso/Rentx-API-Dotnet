using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<CarDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var startDate = request.Result.StartDate.Date;
            var endDate = request.Result.EndDate.Date;

            var carsBetweenDates = await _context.Cars
                    .Include(x => x.CarImages)
                    .Join(_context.CarsAppointments,
                                car => car.Plate,
                                appointment => appointment.Plate,
                                (car, appointment) => new { car, appointment })
                    .Where(x => x.appointment.StartDate >= startDate && x.appointment.EndDate <= endDate)
                    .ToListAsync();

            var carDto = _mapper.Map<List<CarDto>>(carsBetweenDates);

            return Result<List<CarDto>>.Success(carDto);
        }
    }
}
