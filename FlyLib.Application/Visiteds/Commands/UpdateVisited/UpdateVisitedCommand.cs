using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.UpdateVisited
{
    public sealed record UpdateVisitedCommand(int Id, string UserId, int ProvinceId, ICollection<Photo> Photos) : IRequest<Unit>;
}
