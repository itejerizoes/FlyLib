using FluentValidation;

namespace FlyLib.Application.UserVisitedProvinces.Commands.UpdateUserVisitedProvince
{
    public class UpdateUserVisitedProvinceCommandValidator : AbstractValidator<UpdateUserVisitedProvinceCommand>
    {
        public UpdateUserVisitedProvinceCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProvinceId).GreaterThan(0);
            RuleFor(x => x.VisitPhotos).NotNull();
        }
    }
}
