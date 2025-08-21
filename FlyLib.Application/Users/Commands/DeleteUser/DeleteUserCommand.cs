using MediatR;

namespace FlyLib.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public string Id { get; set; }

        public DeleteUserCommand() { }

        public DeleteUserCommand(string id)
        {
            Id = id;
        }
    }
}
