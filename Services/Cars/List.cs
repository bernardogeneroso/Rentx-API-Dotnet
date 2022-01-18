using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

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
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<CarDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cars = await _context.Cars.ProjectTo<CarDto>(_mapper.ConfigurationProvider).ToListAsync();

                return Result<List<CarDto>>.Success(cars);
            }
        }
    }
}