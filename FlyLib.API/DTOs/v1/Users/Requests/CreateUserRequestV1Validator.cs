using FluentValidation;

namespace FlyLib.API.DTOs.v1.Users.Requests
{
    public class CreateUserRequestV1Validator : AbstractValidator<CreateUserRequestV1>
    {
        public CreateUserRequestV1Validator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.");
            RuleFor(x => x.AuthProvider)
                .NotEmpty().WithMessage("El proveedor de autenticación es obligatorio.");
        }
    }
}
