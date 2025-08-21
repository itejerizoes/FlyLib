using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesQuery : IRequest<IEnumerable<CountryDto>>
    {
        public GetAllCountriesQuery() { }
    }
}
