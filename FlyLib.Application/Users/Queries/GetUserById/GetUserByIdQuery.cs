using FlyLib.Application.Users.DTOs;
using MediatR;

namespace FlyLib.Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(string Id) : IRequest<UserDto?>;
}
