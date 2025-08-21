using FlyLib.Application.Common.Exceptions;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Photos.Commands.DeletePhoto
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, Unit>
    {
        private readonly IPhotoRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeletePhotoCommandHandler(IPhotoRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(DeletePhotoCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null)
                throw new NotFoundException($"Foto con id {request.Id} no encontrada.");

            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
