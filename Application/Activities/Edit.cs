using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Edit
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

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _ctx;
            private readonly IMapper _mapper;
            public Handler(DataContext ctx, IMapper mapper) {
                _mapper = mapper;
                _ctx = ctx;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _ctx.Activities.FirstOrDefaultAsync(a => a.ID == request.Activity.ID);

                if (activity == null)
                    return null;

                activity.Title = request.Activity.Title;
                activity.Description = request.Activity.Description;
                activity.Category = request.Activity.Category;
                activity.Date = request.Activity.Date;
                activity.City = request.Activity.City;
                activity.Venue = request.Activity.Venue;

                _ctx.Activities.Update(activity);

                var result = await _ctx.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Не удалось обновить данные о событии");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}