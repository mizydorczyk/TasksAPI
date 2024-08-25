using FluentValidation;

namespace TasksAPI.Models.Validators
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Task title must be specified");
        }
    }
}
