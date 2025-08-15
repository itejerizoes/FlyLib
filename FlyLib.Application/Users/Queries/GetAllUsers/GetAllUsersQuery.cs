using FlyLib.Application.Users.DTOs;
using MediatR;

namespace FlyLib.Application.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery() : IRequest<IEnumerable<UserDto>>;
}
