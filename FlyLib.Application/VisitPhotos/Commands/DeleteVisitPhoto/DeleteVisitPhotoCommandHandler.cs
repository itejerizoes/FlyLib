using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Commands.DeleteVisitPhoto
{
    public class DeleteVisitPhotoCommandHandler : IRequestHandler<DeleteVisitPhotoCommand, Unit>
    {
        private readonly IVisitPhotoRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteVisitPhotoCommandHandler(IVisitPhotoRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(DeleteVisitPhotoCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
