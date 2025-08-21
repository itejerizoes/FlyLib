using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Commands.CreateCountry
{
    public sealed record CreateCountryCommand(string Name, string? IsoCode) : IRequest<CountryDto>;
}
