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
    public class Delete
    {
        public class Command : IRequest<Result<Unit>> 
        {
            public string Id { get; set; }
        }

        public class Handler: IRequestHandler<Command, Result<Unit>> 
        {
            private readonly DataContext _ctx;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext ctx, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _ctx = ctx;
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _ctx.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) 
                    return null;
                
                var photo = user.Photos.FirstOrDefault(x => x.ID == request.Id);

                if (photo == null)
                    return null;
                
                if (photo.IsMain)
                    return Result<Unit>.Failure("Невозможно удалить основное фото");
                
                var result = await _photoAccessor.DeletePhoto(photo.ID);

                if (result == null)
                    return Result<Unit>.Failure("Проблема с удалением фото из Cloudinary");

                user.Photos.Remove(photo);

                var success = await _ctx.SaveChangesAsync() > 0;

                if (success)
                    return Result<Unit>.Success(Unit.Value);
                
                return Result<Unit>.Failure("Проблема с удалением фото из API");
            }
        }
    }
}