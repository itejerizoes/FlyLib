using AutoMapper;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetCountryByName
{
    public class GetCountryByNameQueryHandler : IRequestHandler<GetCountryByNameQuery, CountryDto?>
    {
        private readonly ICountryRepository _repo;
        private readonly IMapper _mapper;

        public GetCountryByNameQueryHandler(ICountryRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<CountryDto?> Handle(GetCountryByNameQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByNameAsync(request.Name, ct);
            return entity is null ? null : _mapper.Map<CountryDto>(entity);
        }
    }
}
