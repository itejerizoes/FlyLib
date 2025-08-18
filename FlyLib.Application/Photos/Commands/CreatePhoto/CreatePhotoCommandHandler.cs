using AutoMapper;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Photos.Commands.CreatePhoto
{
    public class CreatePhotoCommandHandler : IRequestHandler<CreatePhotoCommand, PhotoDto>
    {
        private readonly IPhotoRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreatePhotoCommandHandler(IPhotoRepository repo, IUnitOfWork uow, IMapper mapper)
            => (_repo, _uow, _mapper) = (repo, uow, mapper);

        public async Task<PhotoDto> Handle(CreatePhotoCommand request, CancellationToken ct)
        {
            var entity = new Photo
            {
                Url = request.Url,
                Description = request.Description,
                VisitedId = request.VisitedId
            };
            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return _mapper.Map<PhotoDto>(entity);
        }
    }
}
