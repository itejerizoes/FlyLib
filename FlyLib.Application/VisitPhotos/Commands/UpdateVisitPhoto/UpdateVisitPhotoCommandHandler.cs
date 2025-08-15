using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Commands.UpdateVisitPhoto
{
    public class UpdateVisitPhotoCommandHandler : IRequestHandler<UpdateVisitPhotoCommand, Unit>
    {
        private readonly IVisitPhotoRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateVisitPhotoCommandHandler(IVisitPhotoRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(UpdateVisitPhotoCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null) throw new KeyNotFoundException($"VisitPhoto {request.Id} not found");
            entity.PhotoUrl = request.Url;
            entity.Description = request.Description;
            entity.UserVisitedProvinceId = request.UserVisitedProvinceId;
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
