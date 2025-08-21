using FluentValidation;

namespace FlyLib.API.DTOs.v1.Provinces.Requests
{
    public class UpdateProvinceRequestV1Validator : AbstractValidator<UpdateProvinceRequestV1>
    {
        public UpdateProvinceRequestV1Validator()
        {
            RuleFor(x => x.ProvinceId)
                .GreaterThan(0).WithMessage("El Id de la provincia es obligatorio.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la provincia es obligatorio.");
            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("El Id del país es obligatorio.");
        }
    }
}
