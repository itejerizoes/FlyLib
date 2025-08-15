using FlyLib.Application.UserVisitedProvinces.DTOs;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Queries.GetUserVisitedProvinceById
{
    public sealed record GetUserVisitedProvinceByIdQuery(int Id) : IRequest<UserVisitedProvinceDto?>;
}
