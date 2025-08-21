using FluentValidation;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Visited.Requests;

namespace FlyLib.API.DTOs.v1.Visiteds.Requests
{
    public class CreateVisitedRequestV1Validator : AbstractValidator<CreateVisitedRequestV1>
    {
        public CreateVisitedRequestV1Validator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es obligatorio.");
            RuleFor(x => x.ProvinceId)
                .GreaterThan(0).WithMessage("La provincia es obligatoria.");
            RuleForEach(x => x.Photos)
                .SetValidator(new CreatePhotoRequestV1Validator());
        }
    }
}
