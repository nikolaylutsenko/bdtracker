using BdTracker.Shared.Models.Request;
using FluentValidation;

namespace BdTracker.Back.Validators;
public class RegisterOwnerRequestValidator : AbstractValidator<RegisterOwnerRequest>
{
    public RegisterOwnerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Surname)
            .NotEmpty();
        RuleFor(x => x.CompanyName)
            .NotEmpty();
        RuleFor(x => x.Birthday)
            .NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty();
        RuleFor(x => x.Sex)
            .IsInEnum();
    }
}