using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Unit>
    {
        private readonly ICountryRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateCountryCommandHandler(ICountryRepository repo, IUnitOfWork uow)
            => (_repo, _uow) = (repo, uow);

        public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.CountryId, ct);
            if (entity is null) throw new KeyNotFoundException($"Country {request.CountryId} not found");
            entity.Name = request.Name;
            entity.Iso2 = request.Iso2;
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
