using AutoMapper;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetAllProvinces
{
    public class GetAllProvincesQueryHandler : IRequestHandler<GetAllProvincesQuery, IEnumerable<ProvinceDto>>
    {
        private readonly IProvinceRepository _repo;
        private readonly IMapper _mapper;

        public GetAllProvincesQueryHandler(IProvinceRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<IEnumerable<ProvinceDto>> Handle(GetAllProvincesQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(_mapper.Map<ProvinceDto>);
        }
    }
}
