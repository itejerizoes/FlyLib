using FlyLib.Application.Common.Exceptions;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Unit>
    {
        private readonly ICountryRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteCountryCommandHandler(ICountryRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null)
                throw new NotFoundException($"País con id {request.Id} no encontrado.");

            await _repo.DeleteAsync(request.Id, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
