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
using Microsoft.Extensions.Logging;
using Persistence;
using SQLitePCL;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDTO>>> {}

        public class Handler : IRequestHandler<Query, Result<List<ActivityDTO>>>
        {
            private readonly DataContext _ctx;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _ctx = context;
                _mapper = mapper;
            }
            public async Task<Result<List<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activites = await _ctx.Activities
                                        .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);
                
                return Result<List<ActivityDTO>>.Success(activites);
            }
        }
    }
}