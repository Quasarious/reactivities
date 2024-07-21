using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Application.Activities.Details;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDTO>> {
            public Guid Id { get; set; }
        }
    }

    public class Handler : IRequestHandler<Query, Result<ActivityDTO>>
    {
        private readonly DataContext _ctx;
        private readonly IMapper _mapper;

        public Handler(DataContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public async Task<Result<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _ctx.Activities
                                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync(a => a.ID == request.Id);

            return Result<ActivityDTO>.Success(activity);
        }
    }
}