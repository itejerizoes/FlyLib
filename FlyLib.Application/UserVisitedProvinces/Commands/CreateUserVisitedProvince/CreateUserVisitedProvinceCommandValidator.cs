using FluentValidation;

namespace FlyLib.Application.UserVisitedProvinces.Commands.CreateUserVisitedProvince
{
    public class CreateUserVisitedProvinceCommandValidator : AbstractValidator<CreateUserVisitedProvinceCommand>
    {
        public CreateUserVisitedProvinceCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProvinceId).GreaterThan(0);
            RuleFor(x => x.VisitPhotos).NotNull();
        }
    }
}
