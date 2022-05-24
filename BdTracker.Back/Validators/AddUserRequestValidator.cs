using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BdTracker.Shared.Models.Request;
using FluentValidation;

namespace BdTracker.Back.Validators
{
    public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidator()
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
}