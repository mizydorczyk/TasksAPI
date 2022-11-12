using FluentValidation;

namespace TasksAPI.Models.Validators
{
    public class CreateGroupDtoValidator : AbstractValidator<CreateGroupDto>
    {
        public CreateGroupDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Group name must be specified");
        }
    }
}
