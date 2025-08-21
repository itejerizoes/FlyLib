using MediatR;

namespace FlyLib.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string AuthProvider { get; set; }

        public UpdateUserCommand() { }

        public UpdateUserCommand(string id, string displayName, string authProvider)
        {
            Id = id;
            DisplayName = displayName;
            AuthProvider = authProvider;
        }
    }
}
