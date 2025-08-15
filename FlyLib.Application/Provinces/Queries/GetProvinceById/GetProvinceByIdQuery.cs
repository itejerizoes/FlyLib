using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetProvinceById
{
    public sealed record GetProvinceByIdQuery(int ProvinceId) : IRequest<ProvinceDto?>;
}
