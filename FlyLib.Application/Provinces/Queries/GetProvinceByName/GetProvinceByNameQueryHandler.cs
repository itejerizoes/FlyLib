using AutoMapper;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetProvinceByName
{
    public class GetProvinceByNameQueryHandler : IRequestHandler<GetProvinceByNameQuery, ProvinceDto?>
    {
        private readonly IProvinceRepository _repo;
        private readonly IMapper _mapper;

        public GetProvinceByNameQueryHandler(IProvinceRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<ProvinceDto?> Handle(GetProvinceByNameQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByNameAsync(request.Name, ct);
            return entity is null ? null : _mapper.Map<ProvinceDto>(entity);
        }
    }
}
