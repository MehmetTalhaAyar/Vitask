using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş bırakılamaz");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Minimum 3 karakter giriniz");
            RuleFor(x => x.Name).MaximumLength(30).WithMessage("Maximum 30 karakter giriniz");
            RuleFor(x => x.Description).MinimumLength(5).WithMessage("Min 5 karakter giriniz");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Max 500 karakter giriniz");
        }
    }
}
