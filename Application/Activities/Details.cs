using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Application.Activities.Details;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Activity> {
            public Guid Id { get; set; }
        }
    }

    public class Handler : IRequestHandler<Query, Activity>
    {
        private readonly DataContext _ctx;

        public Handler(DataContext ctx)
        {
            _ctx = ctx;   
        }
        public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _ctx.Activities.FindAsync(request.Id);
        }
    }
}