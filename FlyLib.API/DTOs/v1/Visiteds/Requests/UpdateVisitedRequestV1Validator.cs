using FluentValidation;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Visited.Requests;

namespace FlyLib.API.DTOs.v1.Visiteds.Requests
{
    public class UpdateVisitedRequestV1Validator : AbstractValidator<UpdateVisitedRequestV1>
    {
        public UpdateVisitedRequestV1Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de la visita es obligatorio.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es obligatorio.");
            RuleFor(x => x.ProvinceId)
                .GreaterThan(0).WithMessage("La provincia es obligatoria.");
            RuleForEach(x => x.Photos)
                .SetValidator(new UpdatePhotoRequestV1Validator());
        }
    }
}
