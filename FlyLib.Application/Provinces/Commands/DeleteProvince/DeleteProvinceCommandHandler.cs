using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Provinces.Commands.DeleteProvince
{
    public class DeleteProvinceCommandHandler : IRequestHandler<DeleteProvinceCommand, Unit>
    {
        private readonly IProvinceRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteProvinceCommandHandler(IProvinceRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(DeleteProvinceCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.ProvinceId, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
