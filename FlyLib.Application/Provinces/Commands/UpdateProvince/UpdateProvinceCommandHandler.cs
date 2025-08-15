using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Provinces.Commands.UpdateProvince
{
    public class UpdateProvinceCommandHandler : IRequestHandler<UpdateProvinceCommand, Unit>
    {
        private readonly IProvinceRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateProvinceCommandHandler(IProvinceRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(UpdateProvinceCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.ProvinceId, ct);
            if (entity is null) throw new KeyNotFoundException($"Province {request.ProvinceId} not found");
            entity.Name = request.Name;
            entity.CountryId = request.CountryId;
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
