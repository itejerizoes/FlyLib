using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetAllCountries
{
    public sealed record GetAllCountriesQuery() : IRequest<IEnumerable<CountryDto>>;
}
