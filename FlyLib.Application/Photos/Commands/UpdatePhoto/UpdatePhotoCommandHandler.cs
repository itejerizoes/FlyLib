using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Photos.Commands.UpdatePhoto
{
    public class UpdatePhotoCommandHandler : IRequestHandler<UpdatePhotoCommand, Unit>
    {
        private readonly IPhotoRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdatePhotoCommandHandler(IPhotoRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(UpdatePhotoCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null) throw new KeyNotFoundException($"Photo {request.Id} not found");
            entity.Url = request.Url;
            entity.Description = request.Description;
            entity.VisitedId = request.VisitedId;
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
