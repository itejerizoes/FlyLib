using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetProvinceByName
{
    public sealed record GetProvinceByNameQuery(string Name) : IRequest<ProvinceDto?>;
}
