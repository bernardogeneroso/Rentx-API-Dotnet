using Application.Core;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.CarsAppointments;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

            if (user == null) return Result<Unit>.Failure("Failed to delete car appointment");

            var appointment = await _context.CarsAppointments.FindAsync(request.Id);

            if (appointment == null) return Result<Unit>.Failure("Failed to delete car appointment");

            if (appointment.UserId != user.Id) return Result<Unit>.Failure("Failed to delete car appointment");

            _context.CarsAppointments.Remove(appointment);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete car appointment");

            return Result<Unit>.SuccessNoContent(Unit.Value);
        }
    }
}
