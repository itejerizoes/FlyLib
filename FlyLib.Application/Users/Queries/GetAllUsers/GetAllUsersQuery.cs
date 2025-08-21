using FlyLib.Application.Users.DTOs;
using MediatR;

namespace FlyLib.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public GetAllUsersQuery() { }
    }
}
