using FluentValidation;

namespace FlyLib.API.DTOs.v1.Provinces.Requests
{
    public class CreateProvinceRequestV1Validator : AbstractValidator<CreateProvinceRequestV1>
    {
        public CreateProvinceRequestV1Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la provincia es obligatorio.");
            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("El Id del país es obligatorio.");
        }
    }
}
