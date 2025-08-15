using MediatR;

namespace FlyLib.Application.Users.Commands.DeleteUser
{
    public sealed record DeleteUserCommand(string Id) : IRequest<Unit>;
}
