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
            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
