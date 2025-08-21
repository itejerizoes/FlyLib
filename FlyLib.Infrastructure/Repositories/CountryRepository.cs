using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.Infrastructure.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(FlyLibDbContext context) : base(context) { }

        public async Task<Country?> GetByNameAsync(string name, CancellationToken ct = default)
            => await _context.Set<Country>()
                             .AsNoTracking()
                             .FirstOrDefaultAsync(c => c.Name == name, ct);
    }
}
