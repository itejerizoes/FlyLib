using AutoMapper;
using FlyLib.Application.VisitPhotos.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Queries.GetAllVisitPhotos
{
    public class GetAllVisitPhotosQueryHandler : IRequestHandler<GetAllVisitPhotosQuery, IEnumerable<VisitPhotoDto>>
    {
        private readonly IVisitPhotoRepository _repo;
        private readonly IMapper _mapper;

        public GetAllVisitPhotosQueryHandler(IVisitPhotoRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<IEnumerable<VisitPhotoDto>> Handle(GetAllVisitPhotosQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(_mapper.Map<VisitPhotoDto>);
        }
    }
}
