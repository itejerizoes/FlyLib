using FlyLib.Application.Visiteds.DTOs;
using MediatR;

namespace FlyLib.Application.Visiteds.Queries.GetAllVisiteds
{
    public sealed record GetAllVisitedsQuery() : IRequest<IEnumerable<VisitedDto>>;
}
