using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetCountryByName
{
    public class GetCountryByNameQuery : IRequest<CountryDto?>
    {
        public string Name { get; set; }

        public GetCountryByNameQuery() { }

        public GetCountryByNameQuery(string name)
        {
            Name = name;
        }
    }
}
