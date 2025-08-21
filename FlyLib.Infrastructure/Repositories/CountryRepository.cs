using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.Infrastructure.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly DbContext _ctx;

        public CountryRepository(DbContext ctx) : base(ctx)
            => _ctx = ctx;

        public async Task<Country?> GetByNameAsync(string name, CancellationToken ct = default)
            => await _ctx.Set<Country>()
                         .AsNoTracking()
                         .FirstOrDefaultAsync(c => c.Name == name, ct);
    }
}
