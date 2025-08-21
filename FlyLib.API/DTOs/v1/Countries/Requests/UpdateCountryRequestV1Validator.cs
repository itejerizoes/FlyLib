using FluentValidation;

namespace FlyLib.API.DTOs.v1.Countries.Requests
{
    public class UpdateCountryRequestV1Validator : AbstractValidator<UpdateCountryRequestV1>
    {
        public UpdateCountryRequestV1Validator()
        {
            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("El Id del país es obligatorio.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del país es obligatorio.");
            RuleFor(x => x.IsoCode)
                .NotEmpty().WithMessage("El código ISO es obligatorio.");
        }
    }
}
