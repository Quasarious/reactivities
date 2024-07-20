using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Application.Activities.Details;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<Activity>> {
            public Guid Id { get; set; }
        }
    }

    public class Handler : IRequestHandler<Query, Result<Activity>>
    {
        private readonly DataContext _ctx;

        public Handler(DataContext ctx)
        {
            _ctx = ctx;   
        }
        public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _ctx.Activities.FindAsync(request.Id);

            return Result<Activity>.Success(activity);
        }
    }
}