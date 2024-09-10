using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Task = Entities.Concrete.Task;

namespace Business.ValidationRules
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş bırakılamaz");
            RuleFor(x => x.Name).MinimumLength(5).WithMessage("Minimum 5 karakter");
            RuleFor(x => x.Name).MaximumLength(30).WithMessage("Maximum 30 karakter");
            RuleFor(x => x.Description).MinimumLength(5).WithMessage("Min 5 karakter giriniz");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Max 500 karakter giriniz");
            RuleFor(x => x.Priority).GreaterThan(0).LessThan(10);
        }
    }
}
