using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>> {
            public string TargetUsername { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _ctx;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext ctx, IUserAccessor userAccessor)
            {
                _ctx = ctx;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await _ctx.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

                var target = await _ctx.Users.FirstOrDefaultAsync(u => u.UserName == request.TargetUsername);

                if (target == null)
                    return null;
                
                var following = await _ctx.UserFollowings.FindAsync(observer.Id, target.Id);

                if (following == null) {
                    following = new Domain.UserFollowing 
                    {
                        Observer = observer,
                        Target = target
                    };

                    _ctx.UserFollowings.Add(following);
                } else {
                    _ctx.UserFollowings.Remove(following);
                }

                var success = await _ctx.SaveChangesAsync() > 0;

                if (success)
                    return Result<Unit>.Success(Unit.Value);
                
                return Result<Unit>.Failure("Не удалось обновить подписку");
            }
        }
    }
}