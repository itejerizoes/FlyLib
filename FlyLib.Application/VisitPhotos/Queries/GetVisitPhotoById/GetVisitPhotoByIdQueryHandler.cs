using AutoMapper;
using FlyLib.Application.VisitPhotos.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Queries.GetVisitPhotoById
{
    public class GetVisitPhotoByIdQueryHandler : IRequestHandler<GetVisitPhotoByIdQuery, VisitPhotoDto?>
    {
        private readonly IVisitPhotoRepository _repo;
        private readonly IMapper _mapper;

        public GetVisitPhotoByIdQueryHandler(IVisitPhotoRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<VisitPhotoDto?> Handle(GetVisitPhotoByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            return entity is null ? null : _mapper.Map<VisitPhotoDto>(entity);
        }
    }
}
