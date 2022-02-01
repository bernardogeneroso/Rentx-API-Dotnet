using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.CarsAppointments.DTOs;
using Services.Interfaces;

namespace Services.CarsAppointments;

public class UserScheduledCars
{
    public class Query : IRequest<Result<List<CarScheduledDtoQuery>>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<List<CarScheduledDtoQuery>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly IOriginAccessor _originAccessor;
        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor, IOriginAccessor originAccessor)
        {
            _originAccessor = originAccessor;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<CarScheduledDtoQuery>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<List<CarScheduledDtoQuery>>.Failure("Faield to get your scheduled cars");

            var cars = await _context.Cars.ToListAsync();

            var carsScheduled = await _context.Cars
                                .AsNoTracking()
                                .Join(_context.CarsAppointments,
                                    car => car.Plate,
                                    appointment => appointment.Plate,
                                    (car, appointment) => new { car, appointment })
                                .Where(x => x.appointment.UserId == user.Id)
                                .Select(x => x.car)
                                .OrderByDescending(x => x.Appointments.Any(x => x.StartDate > DateTime.Now))
                                .ProjectTo<CarScheduledDtoQuery>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                                .ToListAsync();

            if (carsScheduled.Count() == 0) return Result<List<CarScheduledDtoQuery>>.Failure("You need to schedule at least one car");

            return Result<List<CarScheduledDtoQuery>>.Success(carsScheduled);
        }
    }
}
