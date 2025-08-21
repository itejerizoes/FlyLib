using FlyLib.Application.Photos.DTOs;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.UpdateVisited
{
    public sealed record UpdateVisitedCommand(int Id, string UserId, int ProvinceId, ICollection<PhotoDto> Photos) : IRequest<Unit>;
}
