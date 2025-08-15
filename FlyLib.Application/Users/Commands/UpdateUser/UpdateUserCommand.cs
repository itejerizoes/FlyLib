using MediatR;

namespace FlyLib.Application.Users.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(string Id, string Email, string? DisplayName) : IRequest<Unit>;
}
