using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.UpdateVisited
{
    public class UpdateVisitedCommandHandler : IRequestHandler<UpdateVisitedCommand, Unit>
    {
        private readonly IVisitedRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateVisitedCommandHandler(IVisitedRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(UpdateVisitedCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null) throw new KeyNotFoundException($"Visited {request.Id} not found");

            entity.UserId = request.UserId;
            entity.ProvinceId = request.ProvinceId;

            entity.Photos = request.Photos
                .Select(p => new Photo(p.Url) { PhotoId = p.PhotoId, Description = p.Description })
                .ToList();

            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
