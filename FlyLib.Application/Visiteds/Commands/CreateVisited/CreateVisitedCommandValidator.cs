using FluentValidation;

namespace FlyLib.Application.Visiteds.Commands.CreateVisited
{
    public class CreateVisitedCommandValidator : AbstractValidator<CreateVisitedCommand>
    {
        public CreateVisitedCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProvinceId).GreaterThan(0);
            RuleFor(x => x.Photos).NotNull();
        }
    }
}
