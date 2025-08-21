using AutoMapper;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Countries.Commands.CreateCountry
{
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, CountryDto>
    {
        private readonly ICountryRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateCountryCommandHandler(ICountryRepository repo, IUnitOfWork uow, IMapper mapper)
            => (_repo, _uow, _mapper) = (repo, uow, mapper);

        public async Task<CountryDto> Handle(CreateCountryCommand request, CancellationToken ct)
        {
            var newEntity = new Country(request.Name) { Name = request.Name, IsoCode = request.IsoCode };
            await _repo.AddAsync(newEntity, ct);
            await _uow.SaveChangesAsync(ct);
            return _mapper.Map<CountryDto>(newEntity);
        }
    }
}
