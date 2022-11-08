using FluentValidation;
using TasksAPI.Models;
using TasksAPI.Entities;

public class RegisterUserDtoValidator : AbstractValidator<RegisterDto>
{

    public RegisterUserDtoValidator(TasksDbContext dbContext)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name must be specified");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name must be specified");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email must be specified")
            .EmailAddress()
            .WithMessage("Incorrect email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password must be specified")
            .MinimumLength(6)
            .WithMessage("Password must be longer than 6 characters");

        RuleFor(x => x.Email)
            .Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("That email is taken");
                }
            });
    }
}