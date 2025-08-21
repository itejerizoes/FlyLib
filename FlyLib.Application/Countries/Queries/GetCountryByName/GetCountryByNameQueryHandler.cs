using AutoMapper;
using FlyLib.Application.Common.Exceptions;
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
            if (entity is null)
                throw new NotFoundException($"País con nombre '{request.Name}' no encontrado.");

            return _mapper.Map<CountryDto>(entity);
        }
    }
}
