using AutoMapper;
using FlyLib.Application.UserVisitedProvinces.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Queries.GetAllUserVisitedProvinces
{
    public class GetAllUserVisitedProvincesQueryHandler : IRequestHandler<GetAllUserVisitedProvincesQuery, IEnumerable<UserVisitedProvinceDto>>
    {
        private readonly IUserVisitedProvinceRepository _repo;
        private readonly IMapper _mapper;

        public GetAllUserVisitedProvincesQueryHandler(IUserVisitedProvinceRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<IEnumerable<UserVisitedProvinceDto>> Handle(GetAllUserVisitedProvincesQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(_mapper.Map<UserVisitedProvinceDto>);
        }
    }
}
