using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Commands.CreateProvince
{
    public sealed record CreateProvinceCommand(string Name, int CountryId) : IRequest<ProvinceDto>;
}
