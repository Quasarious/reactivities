using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
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
            public Handler(DataContext ctx)
            {
                _ctx = ctx;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _ctx.Activities.Add(request.Activity);

                var result = await _ctx.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Не удалось создать событие");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}