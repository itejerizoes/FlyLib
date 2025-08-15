using MediatR;

namespace FlyLib.Application.Countries.Commands.DeleteCountry
{
    public sealed record DeleteCountryCommand(int CountryId) : IRequest<Unit>;
}
