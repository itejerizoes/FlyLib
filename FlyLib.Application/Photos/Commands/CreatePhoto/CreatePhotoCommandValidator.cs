using FluentValidation;

namespace FlyLib.Application.Photos.Commands.CreatePhoto
{
    public class CreatePhotoCommandValidator : AbstractValidator<CreatePhotoCommand>
    {
        public CreatePhotoCommandValidator()
        {
            RuleFor(x => x.Url).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.VisitedId).GreaterThan(0);
        }
    }
}
