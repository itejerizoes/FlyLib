using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetCountryByName
{
    public sealed record GetCountryByNameQuery(string Name) : IRequest<CountryDto?>;
}
