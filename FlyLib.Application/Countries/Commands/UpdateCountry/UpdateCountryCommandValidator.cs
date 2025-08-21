using FluentValidation;

namespace FlyLib.Application.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator()
        {
            RuleFor(x => x.CountryId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.IsoCode).Length(2).When(x => !string.IsNullOrWhiteSpace(x.IsoCode));
        }
    }
}
