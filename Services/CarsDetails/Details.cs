using Application.Core;
using AutoMapper;
using Database;
using FluentValidation;
using MediatR;

namespace Services.CarsDetails;

public class Details
{
    public class Query : IRequest<Result<CarDetailDto>>
    {
        public string Plate { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(x => x.Plate).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Query, Result<CarDetailDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<CarDetailDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var carDetail = await _context.CarsDetails.FindAsync(request.Plate);

            if (carDetail == null) return Result<CarDetailDto>.Failure("Failed to get the car details");

            return Result<CarDetailDto>.Success(_mapper.Map<CarDetailDto>(carDetail));
        }
    }
}
