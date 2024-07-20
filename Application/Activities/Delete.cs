using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>> {
            public Guid ID {get; set;}
        }

        public class Handler : IRequestHandler<Command, Result<Unit>> {
            private readonly DataContext _ctx;
            public Handler(DataContext ctx)
            {
                _ctx = ctx;
                
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _ctx.Activities.FindAsync(request.ID);

                // if (activity == null)
                //     return null;

                _ctx.Remove(activity);

                var result = await _ctx.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Не удалось удалить событие");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}