using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
        private readonly DataContext _ctx;
        private readonly IMapper _mapper;
            public Handler(DataContext ctx, IMapper mapper) {
                _mapper = mapper;
                _ctx = ctx;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _ctx.Activities.FindAsync(request.Activity.ID);

                _mapper.Map(request.Activity, activity);

                await _ctx.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}