using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetCountryById
{
    public class GetCountryByIdQuery : IRequest<CountryDto?>
    {
        public int Id { get; set; }

        public GetCountryByIdQuery() { }

        public GetCountryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
