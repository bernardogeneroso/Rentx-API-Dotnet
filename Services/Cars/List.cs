using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Cars.DTOs;
using Services.Interfaces;

namespace Services.Cars;

public class List
{
    public class Query : IRequest<Result<List<CarDtoQuery>>>
    {
        public string Search { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<CarDtoQuery>>>
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

        public async Task<Result<List<CarDtoQuery>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Cars
                                    .ProjectTo<CarDtoQuery>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                                    .OrderByDescending(x => x.CreatedAt)
                                    .AsNoTracking()
                                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x => x.Brand.Contains(request.Search, StringComparison.OrdinalIgnoreCase) || x.Model.Contains(request.Search, StringComparison.OrdinalIgnoreCase));
            }

            return Result<List<CarDtoQuery>>.Success(await query.ToListAsync(cancellationToken));
        }
    }
}
