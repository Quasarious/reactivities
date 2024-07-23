using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result<Unit>> 
        {
            public string ID { get; set; }
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
                var user = await _ctx.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null)
                    return null;
                
                var photo = user.Photos.FirstOrDefault(x => x.ID == request.ID);

                if (photo == null)
                    return null;
                
                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

                if (currentMain != null)
                    currentMain.IsMain = false;
                
                photo.IsMain = true;

                var success = await _ctx.SaveChangesAsync() > 0;

                if (success)
                    return Result<Unit>.Success(Unit.Value);
                
                return Result<Unit>.Failure("Проблема со сменой основного фото");
            }
        }
    }
}