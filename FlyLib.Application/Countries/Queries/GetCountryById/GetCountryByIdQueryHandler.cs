using AutoMapper;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Countries.Queries.GetCountryById
{
    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDto?>
    {
        private readonly ICountryRepository _repo;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(ICountryRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<CountryDto?> Handle(GetCountryByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            return entity is null ? null : _mapper.Map<CountryDto>(entity);
        }
    }
}
