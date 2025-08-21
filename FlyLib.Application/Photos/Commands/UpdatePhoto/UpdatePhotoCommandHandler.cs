using FlyLib.Application.Common.Exceptions;
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
            var entity = await _repo.GetByIdAsync(request.PhotoId, ct);
            if (entity is null)
                throw new NotFoundException($"Foto con id {request.PhotoId} no encontrada.");

            entity.Url = request.Url;
            entity.Description = request.Description;
            entity.VisitedId = request.VisitedId;
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
