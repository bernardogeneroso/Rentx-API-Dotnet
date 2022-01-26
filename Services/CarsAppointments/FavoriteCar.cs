using Application.Core;
using AutoMapper;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<FavoriteCarDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<FavoriteCarDto>.Failure("Faield to get your favorite car");

            var favoriteCar = await _context.CarsAppointments.OrderByDescending(x => x.Plate).GroupBy(x => x.Plate).Select(x => new
            {
                Plate = x.Key,
                Count = x.Count()
            }).FirstOrDefaultAsync();

            if (favoriteCar == null) return Result<FavoriteCarDto>.Failure("Faield to get your favorite car");

            var sumOfFavoriteCarRentalDays = _context.CarsAppointments.Where(x => x.Plate == favoriteCar.Plate).Select(x => new
            {
                Days = x.EndDate.Subtract(x.StartDate).Days
            }).ToListAsync().Result.Sum(x => x.Days);

            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Plate == favoriteCar.Plate);

            if (car == null) return Result<FavoriteCarDto>.Failure("Faield to get your favorite car");

            var favoriteCarDto = new FavoriteCarDto
            {
                TotalRentalDays = sumOfFavoriteCarRentalDays
            };

            _mapper.Map(car, favoriteCarDto);

            return Result<FavoriteCarDto>.Success(favoriteCarDto);
        }
    }
}
