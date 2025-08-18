using FlyLib.Application.Visiteds.DTOs;
using MediatR;

namespace FlyLib.Application.Visiteds.Queries.GetVisitedById
{
    public sealed record GetVisitedByIdQuery(int Id) : IRequest<VisitedDto?>;
}
