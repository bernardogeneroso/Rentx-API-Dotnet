using Application.Core;
using AutoMapper;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Cars;

public class FavoriteCar
{
    public class Command : IRequest<Result<CarDto>>
    {
    }

    public class Handler : IRequestHandler<Command, Result<CarDto>>
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

        public async Task<Result<CarDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<CarDto>.Failure("Faield to get your favorite car");

            var favoriteCar = await _context.CarsAppointments.OrderByDescending(x => x.Plate).GroupBy(x => x.Plate).Select(x => new
            {
                Plate = x.Key,
            }).FirstOrDefaultAsync();

            if (favoriteCar == null) return Result<CarDto>.Failure("Faield to get your favorite car");

            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Plate == favoriteCar.Plate);

            if (car == null) return Result<CarDto>.Failure("Faield to get your favorite car");

            var carDto = _mapper.Map<CarDto>(car);

            return Result<CarDto>.Success(carDto);
        }
    }
}
