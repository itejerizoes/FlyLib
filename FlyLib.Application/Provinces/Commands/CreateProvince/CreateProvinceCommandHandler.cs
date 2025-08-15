using AutoMapper;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Provinces.Commands.CreateProvince
{
    public class CreateProvinceCommandHandler : IRequestHandler<CreateProvinceCommand, ProvinceDto>
    {
        private readonly IProvinceRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateProvinceCommandHandler(IProvinceRepository repo, IUnitOfWork uow, IMapper mapper)
            => (_repo, _uow, _mapper) = (repo, uow, mapper);

        public async Task<ProvinceDto> Handle(CreateProvinceCommand request, CancellationToken ct)
        {
            var entity = new Province { Name = request.Name, CountryId = request.CountryId };
            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return _mapper.Map<ProvinceDto>(entity);
        }
    }
}
