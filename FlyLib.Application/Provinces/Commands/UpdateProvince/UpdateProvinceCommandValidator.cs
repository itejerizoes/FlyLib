using FluentValidation;
namespace FlyLib.Application.Provinces.Commands.UpdateProvince
{
    public class UpdateProvinceCommandValidator : AbstractValidator<UpdateProvinceCommand>
    {
        public UpdateProvinceCommandValidator()
        {
            RuleFor(x => x.ProvinceId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CountryId).GreaterThan(0);
        }
    }
}
