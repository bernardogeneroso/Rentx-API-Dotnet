using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Cars.DTOs;
using Services.CarsAppointments.DTOs;
using Services.Interfaces;

namespace Services.CarsAppointments;

public class FavoriteCar
{
    public class Query : IRequest<Result<FavoriteCarDtoQuery>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<FavoriteCarDtoQuery>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly IOriginAccessor _originAccessor;
        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor, IOriginAccessor originAccessor)
        {
            _originAccessor = originAccessor;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<FavoriteCarDtoQuery>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<FavoriteCarDtoQuery>.Failure("Faield to get your favorite car");

            var allPossibleFavoriteCars = await _context.CarsAppointments
            .AsNoTracking()
            .Where(x => x.UserId == user.Id)
            .GroupBy(x => x.Plate)
            .Select(x => new
            {
                Appointment = x.ToList(),
                Plate = x.Key,
                Count = x.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

            if (allPossibleFavoriteCars.Count() == 0) return Result<FavoriteCarDtoQuery>.Failure("You needed to schedule at least one car");

            var favoriteCarRentalDays = allPossibleFavoriteCars
            .Select(x => new
            {
                x.Plate,
                Days = x.Appointment.Sum(y => (y.EndDate - y.StartDate).Days + 1)
            })
            .OrderByDescending(x => x.Days)
            .ToList()
            .FirstOrDefault();

            var car = await _context.Cars.ProjectTo<CarDtoQuery>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() }).FirstOrDefaultAsync(x => x.Plate == favoriteCarRentalDays.Plate);

            if (car == null) return Result<FavoriteCarDtoQuery>.Failure("Faield to get your favorite car");

            var favoriteCarDtoQuery = new FavoriteCarDtoQuery
            {
                TotalRentalDays = favoriteCarRentalDays.Days,
            };

            _mapper.Map(car, favoriteCarDtoQuery);

            return Result<FavoriteCarDtoQuery>.Success(favoriteCarDtoQuery);
        }
    }
}