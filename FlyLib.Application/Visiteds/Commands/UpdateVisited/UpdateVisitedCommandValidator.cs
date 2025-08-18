using FluentValidation;

namespace FlyLib.Application.Visiteds.Commands.UpdateVisited
{
    public class UpdateVisitedCommandValidator : AbstractValidator<UpdateVisitedCommand>
    {
        public UpdateVisitedCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProvinceId).GreaterThan(0);
            RuleFor(x => x.Photos).NotNull();
        }
    }
}
