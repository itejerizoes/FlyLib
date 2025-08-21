using FluentValidation;

namespace FlyLib.Application.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.IsoCode).Length(2).When(x => !string.IsNullOrWhiteSpace(x.IsoCode));
        }
    }
}
