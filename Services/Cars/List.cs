using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Cars
{
    public class List
    {
        public class Query : IRequest<Result<List<CarDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<CarDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IOriginAccessor _originAccessor;
            public Handler(DataContext context, IOriginAccessor originAccessor, IMapper mapper)
            {
                _originAccessor = originAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<CarDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cars = await _context.Cars
                                        .Include(c => c.CarImages)
                                        .AsSplitQuery()
                                        .ProjectTo<CarDto>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                                        .ToListAsync();

                return Result<List<CarDto>>.Success(cars);
            }
        }
    }
}