using FlyLib.Application.UserVisitedProvinces.DTOs;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Queries.GetAllUserVisitedProvinces
{
    public sealed record GetAllUserVisitedProvincesQuery() : IRequest<IEnumerable<UserVisitedProvinceDto>>;
}
