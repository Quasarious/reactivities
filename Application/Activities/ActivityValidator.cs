using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using FluentValidation;

namespace Application.Activities
{
    public class ActivityValidator : AbstractValidator<Activity> 
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(5).WithMessage("Длина заголовка должна быть больше 5 символов");
            
            RuleFor(x => x.Description)
                .NotEmpty();
            
            RuleFor(x => x.Date)
                .NotEmpty();

            RuleFor(x => x.Category)
                .NotEmpty();

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Venue)
                .NotEmpty();
        }
    }
}