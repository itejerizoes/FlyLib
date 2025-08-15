using AutoMapper;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetProvinceById
{
    public class GetProvinceByIdQueryHandler : IRequestHandler<GetProvinceByIdQuery, ProvinceDto?>
    {
        private readonly IProvinceRepository _repo;
        private readonly IMapper _mapper;

        public GetProvinceByIdQueryHandler(IProvinceRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<ProvinceDto?> Handle(GetProvinceByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.ProvinceId, ct);
            return entity is null ? null : _mapper.Map<ProvinceDto>(entity);
        }
    }
}
