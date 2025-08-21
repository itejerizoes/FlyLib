using FluentValidation;

namespace FlyLib.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.DisplayName).MaximumLength(100);
        }
    }
}
