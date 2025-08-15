using FluentValidation;

namespace FlyLib.Application.Provinces.Commands.CreateProvince
{
    public class CreateProvinceCommandValidator : AbstractValidator<CreateProvinceCommand>
    {
        public CreateProvinceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CountryId).GreaterThan(0);
        }
    }
}
