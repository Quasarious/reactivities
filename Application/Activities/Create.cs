using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>> {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>> {
            private readonly DataContext _ctx;
            private readonly IUserAccessor _userAccessor; 
            public Handler(DataContext ctx, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _ctx = ctx;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _ctx.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                var attendee = new ActivityAttendee{
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendees.Add(attendee);

                _ctx.Activities.Add(request.Activity);

                var result = await _ctx.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Не удалось создать событие");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}