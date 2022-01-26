using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services.CarsAppointments;

public class UserScheduledCars
{
    public class Query : IRequest<Result<List<CarScheduledDto>>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<List<CarScheduledDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<CarScheduledDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<List<CarScheduledDto>>.Failure("Faield to get your scheduled cars");

            var cars = await _context.Cars.ToListAsync();

            var carsScheduled = await _context.Cars
                                .Join(_context.CarsAppointments,
                                    car => car.Plate,
                                    appointment => appointment.Plate,
                                    (car, appointment) => new { car, appointment })
                                .Where(x => x.appointment.UserId == user.Id)
                                .Select(x => x.car)
                                .OrderByDescending(x => x.CarAppointments.Any(x => x.StartDate > DateTime.Now))
                                .ProjectTo<CarScheduledDto>(_mapper.ConfigurationProvider)
                                .ToListAsync();

            return Result<List<CarScheduledDto>>.Success(carsScheduled);
        }
    }
}
