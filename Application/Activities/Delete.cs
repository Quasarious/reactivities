using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest {
            public Guid ID {get; set;}
        }

        public class Handler : IRequestHandler<Command> {
            private readonly DataContext _ctx;
            public Handler(DataContext ctx)
            {
                _ctx = ctx;
                
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _ctx.Activities.FindAsync(request.ID);

                _ctx.Remove(activity);

                await _ctx.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}