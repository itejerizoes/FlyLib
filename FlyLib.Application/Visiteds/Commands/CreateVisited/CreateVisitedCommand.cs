using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.CreateVisited
{
    public sealed record CreateVisitedCommand(string UserId, int ProvinceId, ICollection<Photo> Photos) : IRequest<VisitedDto>;
}
