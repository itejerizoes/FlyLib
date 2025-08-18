using AutoMapper;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Visiteds.Queries.GetAllVisiteds
{
    public class GetAllVisitedsQueryHandler : IRequestHandler<GetAllVisitedsQuery, IEnumerable<VisitedDto>>
    {
        private readonly IVisitedRepository _repo;
        private readonly IMapper _mapper;

        public GetAllVisitedsQueryHandler(IVisitedRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<IEnumerable<VisitedDto>> Handle(GetAllVisitedsQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(_mapper.Map<VisitedDto>);
        }
    }
}
