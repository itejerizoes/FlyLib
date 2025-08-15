using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteUserCommandHandler(IUserRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
