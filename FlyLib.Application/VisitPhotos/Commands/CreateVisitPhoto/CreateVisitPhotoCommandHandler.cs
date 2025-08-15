using AutoMapper;
using FlyLib.Application.VisitPhotos.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Commands.CreateVisitPhoto
{
    public class CreateVisitPhotoCommandHandler : IRequestHandler<CreateVisitPhotoCommand, VisitPhotoDto>
    {
        private readonly IVisitPhotoRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateVisitPhotoCommandHandler(IVisitPhotoRepository repo, IUnitOfWork uow, IMapper mapper)
            => (_repo, _uow, _mapper) = (repo, uow, mapper);

        public async Task<VisitPhotoDto> Handle(CreateVisitPhotoCommand request, CancellationToken ct)
        {
            var entity = new VisitPhoto
            {
                PhotoUrl = request.PhotoUrl,
                Description = request.Description,
                UserVisitedProvinceId = request.UserVisitedProvinceId
            };
            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return _mapper.Map<VisitPhotoDto>(entity);
        }
    }
}
