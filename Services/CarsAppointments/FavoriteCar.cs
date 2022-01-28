using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Cars;
using Services.Interfaces;

namespace Services.CarsAppointments;

public class FavoriteCar
{
    public class Query : IRequest<Result<FavoriteCarDto>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<FavoriteCarDto>>
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

        public async Task<Result<FavoriteCarDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<FavoriteCarDto>.Failure("Faield to get your favorite car");

            var allPossibleFavoriteCars = await _context.CarsAppointments.Where(x => x.UserId == user.Id)
            .GroupBy(x => x.Plate)
            .Select(x => new
            {
                Appointment = x.ToList(),
                Plate = x.Key,
                Count = x.Count()
            }).OrderByDescending(x => x.Count).ToListAsync();

            if (allPossibleFavoriteCars.Count() == 0) return Result<FavoriteCarDto>.Failure("You needed to schedule at least one car");

            var favoriteCarRentalDays = allPossibleFavoriteCars.Select(x => new
            {
                x.Plate,
                Days = x.Appointment.Sum(y => (y.EndDate - y.StartDate).Days + 1)
            }).OrderByDescending(x => x.Days).ToList().FirstOrDefault();

            var car = await _context.Cars.ProjectTo<CarDto>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() }).FirstOrDefaultAsync(x => x.Plate == favoriteCarRentalDays.Plate);

            if (car == null) return Result<FavoriteCarDto>.Failure("Faield to get your favorite car");

            var favoriteCarDto = new FavoriteCarDto
            {
                TotalRentalDays = favoriteCarRentalDays.Days,
            };

            _mapper.Map(car, favoriteCarDto);

            return Result<FavoriteCarDto>.Success(favoriteCarDto);
        }
    }
}