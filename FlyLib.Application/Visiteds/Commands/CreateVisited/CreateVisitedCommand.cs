using FlyLib.Application.Photos.DTOs;
using FlyLib.Application.Visiteds.DTOs;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.CreateVisited
{
    public sealed record CreateVisitedCommand(string UserId, int ProvinceId, ICollection<PhotoDto> Photos) : IRequest<VisitedDto>;
}
