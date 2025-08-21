using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Commands.UpdateCountry
{
    public sealed record UpdateCountryCommand(int CountryId, string Name, string? IsoCode) : IRequest<Unit>;
}
