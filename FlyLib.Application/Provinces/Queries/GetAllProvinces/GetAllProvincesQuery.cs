using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetAllProvinces
{
    public sealed record GetAllProvincesQuery() : IRequest<IEnumerable<ProvinceDto>>;
}
