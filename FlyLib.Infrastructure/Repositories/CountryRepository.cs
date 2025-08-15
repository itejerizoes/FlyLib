using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.Infrastructure.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly FlyLibDbContext _ctx;
        public CountryRepository(FlyLibDbContext ctx) : base(ctx) => _ctx = ctx;

        public Task<Country?> GetByNameAsync(string name, CancellationToken ct = default)
            => _ctx.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name, ct);
    }
}
