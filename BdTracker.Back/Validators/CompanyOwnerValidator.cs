using BdTracker.Shared.Models.Request;
using FluentValidation;

namespace BdTracker.Back.Validators;

public class CompanyOwnerValidator : AbstractValidator<CompanyOwnerRequest>
{
    public CompanyOwnerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can not be empty")
            .Length(2, 50).WithMessage("Name must be greater 1 and less 50 chars")
            .Matches(@"^[a-zA-Z \-']+$").WithMessage("Name can only contain this characters - a-zA-Z '-");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname can not be empty")
            .Length(2, 50).WithMessage("Surname must be greater 1 and less 50 chars")
            .Matches(@"^[a-zA-Z \-']+$").WithMessage("Surname can only contain this characters - a-zA-Z '-");

        RuleFor(x => x.BirthDay)
            .NotEmpty().WithMessage("You must provide date");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("You must provide email")
            .EmailAddress().WithMessage("You must provide valid email")
            .Matches(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$").WithMessage("You must provide valid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("You must provide password")
            .Length(6, 30).WithMessage("Password must be greater then 6 chars and less 30 chars");

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("You must provide company name")
            .Length(2, 150).WithMessage("Company name must be greater 1 and less 150 chars")
            .Matches(@"^[a-zA-Z \-]+$").WithMessage("You can use only a-zA-Z - chars");

        RuleFor(x => x.PositionName)
            .NotEmpty().WithMessage("You must provide position name")
            .Length(2, 100).WithMessage("Position name must be greater 1 and less 100 chars")
            .Matches(@"^[a-zA-Z \-]+$").WithMessage("You can use only a-zA-Z - chars");
    }
}
