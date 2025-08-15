using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetCountryById
{
    public sealed record GetCountryByIdQuery(int Id) : IRequest<CountryDto?>;
}
