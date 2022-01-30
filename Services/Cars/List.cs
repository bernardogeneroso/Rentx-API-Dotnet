using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Cars;

public class List
{
    public class Query : IRequest<Result<List<CarDto>>>
    {
        public string Search { get; set; }
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
            var query = _context.Cars
                                    .ProjectTo<CarDto>(_mapper.ConfigurationProvider, new { currentOrigin = _originAccessor.GetOrigin() })
                                    .OrderByDescending(x => x.CreatedAt)
                                    .AsNoTracking()
                                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(c => c.Brand.ToLower().Contains(request.Search.ToLower()) || c.Model.ToLower().Contains(request.Search.ToLower()));
            }

            return Result<List<CarDto>>.Success(await query.ToListAsync());
        }
    }
}
