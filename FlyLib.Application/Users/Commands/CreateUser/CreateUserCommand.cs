using FlyLib.Application.Users.DTOs;
using MediatR;

namespace FlyLib.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string DisplayName { get; set; }
        public string AuthProvider { get; set; }

        public CreateUserCommand() { }

        public CreateUserCommand(string displayName, string authProvider)
        {
            DisplayName = displayName;
            AuthProvider = authProvider;
        }
    }
}
