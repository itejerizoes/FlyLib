using FluentValidation;

namespace FlyLib.API.DTOs.v1.Countries.Requests
{
    public class CreateCountryRequestV1Validator : AbstractValidator<CreateCountryRequestV1>
    {
        public CreateCountryRequestV1Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del país es obligatorio.");
            RuleFor(x => x.IsoCode)
                .NotEmpty().WithMessage("El código ISO es obligatorio.");
        }
    }
}
