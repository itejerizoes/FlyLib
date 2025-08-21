using MediatR;

namespace FlyLib.Application.Visiteds.Commands.DeleteVisited
{
    public class DeleteVisitedCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteVisitedCommand() { }

        public DeleteVisitedCommand(int id)
        {
            Id = id;
        }
    }
}
