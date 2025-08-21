using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.Infrastructure.Repositories
{
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        public ProvinceRepository(FlyLibDbContext context) : base(context) { }

        public Task<Province?> GetByNameAsync(string name, CancellationToken ct = default)
            => _context.Set<Province>().AsNoTracking().FirstOrDefaultAsync(c => c.Name == name, ct);
    }
}
