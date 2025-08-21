using FluentValidation;

namespace FlyLib.Application.Photos.Commands.UpdatePhoto
{
    public class UpdatePhotoCommandValidator : AbstractValidator<UpdatePhotoCommand>
    {
        public UpdatePhotoCommandValidator()
        {
            RuleFor(x => x.PhotoId).GreaterThan(0);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.VisitedId).GreaterThan(0);
        }
    }
}
