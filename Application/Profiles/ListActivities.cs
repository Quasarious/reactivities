using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListActivities
    {
        public class Query : IRequest<Result<List<UserActivityDTO>>> {
            public string Username { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserActivityDTO>>>
        {
            private readonly DataContext _ctx;
            private readonly IMapper _mapper;

            public Handler(DataContext ctx, IMapper mapper)
            {
                _ctx = ctx;
                _mapper = mapper;
            }

            public async Task<Result<List<UserActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query =  _ctx.ActivityAttendees
                    .Where(a => a.AppUser.UserName == request.Username)
                    .ProjectTo<UserActivityDTO>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                switch (request.Predicate) {
                    case "past":
                        query = query.Where(a => a.Date <= DateTime.Now);
                        break;
                    case "future":
                        query = query.Where(a => a.Date >= DateTime.Now);
                        break;
                    case "hosting":
                        query = query.Where(a => a.HostUsername == request.Username);
                        break;
                    default:
                        query = query.Where(a => a.Date >= DateTime.Now);
                        break;
                }

                return Result<List<UserActivityDTO>>.Success(await query.ToListAsync());
            }
        }
    }
}