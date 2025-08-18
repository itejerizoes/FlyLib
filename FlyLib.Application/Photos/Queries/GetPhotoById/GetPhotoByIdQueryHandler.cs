using AutoMapper;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Photos.Queries.GetPhotoById
{
    public class GetPhotoByIdQueryHandler : IRequestHandler<GetPhotoByIdQuery, PhotoDto?>
    {
        private readonly IPhotoRepository _repo;
        private readonly IMapper _mapper;

        public GetPhotoByIdQueryHandler(IPhotoRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<PhotoDto?> Handle(GetPhotoByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            return entity is null ? null : _mapper.Map<PhotoDto>(entity);
        }
    }
}
