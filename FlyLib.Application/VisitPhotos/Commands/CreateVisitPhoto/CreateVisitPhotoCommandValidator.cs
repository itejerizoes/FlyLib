using FluentValidation;

namespace FlyLib.Application.VisitPhotos.Commands.CreateVisitPhoto
{
    public class CreateVisitPhotoCommandValidator : AbstractValidator<CreateVisitPhotoCommand>
    {
        public CreateVisitPhotoCommandValidator()
        {
            RuleFor(x => x.PhotoUrl).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.UserVisitedProvinceId).GreaterThan(0);
        }
    }
}
