using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Add
    {
        public class Command : IRequest<Result<Photo>> 
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Photo>> 
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

            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _ctx.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null)
                    return null;
                
                var PhotoUploadResult = await _photoAccessor.AddPhoto(request.File);

                var photo = new Photo 
                {
                    Url = PhotoUploadResult.Url,
                    ID = PhotoUploadResult.PublicId
                };

                if (!user.Photos.Any(x => x.IsMain))
                    photo.IsMain = true;

                user.Photos.Add(photo);

                var result = await _ctx.SaveChangesAsync() > 0;

                if (result)
                    return Result<Photo>.Success(photo);

                return Result<Photo>.Failure("Не удалось добавить фото");                
            }
        }
    }
}