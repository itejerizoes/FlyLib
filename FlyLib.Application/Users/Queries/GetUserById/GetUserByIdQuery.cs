using FlyLib.Application.Users.DTOs;
using MediatR;

namespace FlyLib.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public string Id { get; set; }

        public GetUserByIdQuery() { }

        public GetUserByIdQuery(string id)
        {
            Id = id;
        }
    }
}
