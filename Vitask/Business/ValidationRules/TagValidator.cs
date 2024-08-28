using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules
{
    public class TagValidator : AbstractValidator<Tag>
    {
        public TagValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tag ismi boş bırakılamaz.");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Minimum 3 karakter giriniz");
            RuleFor(x => x.Name).MaximumLength(30).WithMessage("Maximum 30 karakter giriniz");
        }
    }
}
