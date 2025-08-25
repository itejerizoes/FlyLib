using FluentValidation;

namespace FlyLib.API.DTOs.v1.Auth.Request
{
    public class RegisterRequestV1Validator : AbstractValidator<RegisterRequestV1>
    {
        public RegisterRequestV1Validator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
