using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Commands.UpdateUserVisitedProvince
{
    public class UpdateUserVisitedProvinceCommandHandler : IRequestHandler<UpdateUserVisitedProvinceCommand, Unit>
    {
        private readonly IUserVisitedProvinceRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateUserVisitedProvinceCommandHandler(IUserVisitedProvinceRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(UpdateUserVisitedProvinceCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null) throw new KeyNotFoundException($"UserVisitedProvince {request.Id} not found");
            entity.UserId = request.UserId;
            entity.ProvinceId = request.ProvinceId;
            entity.VisitPhotos = request.VisitPhotos;
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
