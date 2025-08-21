using FluentValidation;

namespace FlyLib.API.DTOs.v1.Users.Requests
{
    public class UpdateUserRequestV1Validator : AbstractValidator<UpdateUserRequestV1>
    {
        public UpdateUserRequestV1Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El Id de usuario es obligatorio.");
            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.");
            RuleFor(x => x.AuthProvider)
                .NotEmpty().WithMessage("El proveedor de autenticación es obligatorio.");
        }
    }
}
