using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Commands.DeleteUserVisitedProvince
{
    public class DeleteUserVisitedProvinceCommandHandler : IRequestHandler<DeleteUserVisitedProvinceCommand, Unit>
    {
        private readonly IUserVisitedProvinceRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteUserVisitedProvinceCommandHandler(IUserVisitedProvinceRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(DeleteUserVisitedProvinceCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
