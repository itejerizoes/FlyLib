using AutoMapper;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
    {
        private readonly ICountryRepository _repo;
        private readonly IMapper _mapper;

        public GetAllCountriesQueryHandler(ICountryRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken ct)
        {
            var data = await _repo.GetAllAsync(ct);
            return data.Select(e => _mapper.Map<CountryDto>(e));
        }
    }
}
