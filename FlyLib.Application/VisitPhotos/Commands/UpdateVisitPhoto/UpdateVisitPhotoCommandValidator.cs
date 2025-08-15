using FluentValidation;

namespace FlyLib.Application.VisitPhotos.Commands.UpdateVisitPhoto
{
    public class UpdateVisitPhotoCommandValidator : AbstractValidator<UpdateVisitPhotoCommand>
    {
        public UpdateVisitPhotoCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.UserVisitedProvinceId).GreaterThan(0);
        }
    }
}
