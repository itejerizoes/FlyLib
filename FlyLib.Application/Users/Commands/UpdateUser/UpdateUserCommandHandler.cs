using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateUserCommandHandler(IUserRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null) throw new KeyNotFoundException($"User {request.Id} not found");
            entity.Email = request.Email;
            entity.DisplayName = request.DisplayName;
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
