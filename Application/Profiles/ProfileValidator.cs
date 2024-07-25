using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Profiles
{
    public class ProfileValidator : AbstractValidator<Profile> 
    {
        public ProfileValidator()
        {
            RuleFor(a => a.DisplayName)
                .NotEmpty().WithMessage("Поле не может быть пустым")
                .MaximumLength(50).WithMessage("Имя не может быть больше 50 символов");
            RuleFor(a => a.Bio)
                .MaximumLength(5000).WithMessage("Био не может быть больше 5000 символов");
        }
    }
}