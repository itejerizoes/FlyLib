using AutoMapper;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Photos.Queries.GetAllPhotos
{
    public class GetAllPhotosQueryHandler : IRequestHandler<GetAllPhotosQuery, IEnumerable<PhotoDto>>
    {
        private readonly IPhotoRepository _repo;
        private readonly IMapper _mapper;

        public GetAllPhotosQueryHandler(IPhotoRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<IEnumerable<PhotoDto>> Handle(GetAllPhotosQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct: ct);
            return items.Select(_mapper.Map<PhotoDto>);
        }
    }
}
