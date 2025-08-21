using FluentValidation;

namespace FlyLib.API.DTOs.v1.Photos.Requests
{
    public class CreatePhotoRequestV1Validator : AbstractValidator<CreatePhotoRequestV1>
    {
        public CreatePhotoRequestV1Validator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("La URL de la foto es obligatoria.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("La URL debe ser válida.");
            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("La descripción no puede superar los 200 caracteres.");
            RuleFor(x => x.VisitedId)
                .GreaterThan(0).WithMessage("El Id de la visita es obligatorio.");
        }
    }
}
