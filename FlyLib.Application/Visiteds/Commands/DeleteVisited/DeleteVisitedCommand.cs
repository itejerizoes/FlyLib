using MediatR;

namespace FlyLib.Application.Visiteds.Commands.DeleteVisited
{
    public sealed record DeleteVisitedCommand(int Id) : IRequest<Unit>;
}
