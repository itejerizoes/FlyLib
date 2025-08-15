using FlyLib.Application.Users.DTOs;
using MediatR;

namespace FlyLib.Application.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand(string Email, string? DisplayName) : IRequest<UserDto>;
}
