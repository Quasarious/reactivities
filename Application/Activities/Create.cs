using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest {
            public Activity Activity { get; set; }
        }


        public class Handler : IRequestHandler<Command> {
            private readonly DataContext _ctx;
            public Handler(DataContext ctx)
            {
                _ctx = ctx;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _ctx.Activities.Add(request.Activity);

                await _ctx.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}