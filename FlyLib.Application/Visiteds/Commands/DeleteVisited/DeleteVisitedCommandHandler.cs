using FlyLib.Application.Common.Exceptions;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.DeleteVisited
{
    public class DeleteVisitedCommandHandler : IRequestHandler<DeleteVisitedCommand, Unit>
    {
        private readonly IVisitedRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteVisitedCommandHandler(IVisitedRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(DeleteVisitedCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null)
                throw new NotFoundException($"Visitado con id {request.Id} no encontrado.");

            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
