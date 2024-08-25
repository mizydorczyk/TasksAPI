using FluentValidation;
using TasksAPI.Entities;

namespace TasksAPI.Models.Validators;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Password must be specified")
            .MinimumLength(6)
            .WithMessage("Password must be longer than 6 characters");
    }
}