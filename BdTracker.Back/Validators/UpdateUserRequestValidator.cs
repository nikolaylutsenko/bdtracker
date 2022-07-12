using BdTracker.Shared.Models.Request;
using FluentValidation;

namespace BdTracker.Back.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
                .NotEmpty();
        RuleFor(x => x.Surname)
            .NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty();
        RuleFor(x => x.BirthDay)
            .NotEmpty();
        RuleFor(x => x.Sex)
            .IsInEnum();
        RuleFor(x => x.PositionName)
            .NotEmpty();
    }
}