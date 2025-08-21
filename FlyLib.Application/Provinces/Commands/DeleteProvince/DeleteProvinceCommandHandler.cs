using FlyLib.Application.Common.Exceptions;
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
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null)
                throw new NotFoundException($"Provincia con id {request.Id} no encontrada.");

            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
