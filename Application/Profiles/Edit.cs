using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> {
            public Profile Profile {get; set;}
        }

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator()
            {
                RuleFor(x => x.Profile).SetValidator(new ProfileValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _ctx;

            public Handler(DataContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _ctx.Users.FirstOrDefaultAsync(u => u.UserName == request.Profile.Username);

                if (user == null)
                    return null;

                if (user.DisplayName == request.Profile.DisplayName &&
                    user.Bio == request.Profile.Bio)
                    return Result<Unit>.Failure("Данные не были изменены");
                
                user.DisplayName = request.Profile.DisplayName;
                user.Bio = request.Profile.Bio;

                var result = await _ctx.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Не удалось обновить данные профиля");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}