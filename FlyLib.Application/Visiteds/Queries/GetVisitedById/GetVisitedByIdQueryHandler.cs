using AutoMapper;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Visiteds.Queries.GetVisitedById
{
    public class GetVisitedByIdQueryHandler : IRequestHandler<GetVisitedByIdQuery, VisitedDto?>
    {
        private readonly IVisitedRepository _repo;
        private readonly IMapper _mapper;

        public GetVisitedByIdQueryHandler(IVisitedRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<VisitedDto?> Handle(GetVisitedByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            return entity is null ? null : _mapper.Map<VisitedDto>(entity);
        }
    }
}
