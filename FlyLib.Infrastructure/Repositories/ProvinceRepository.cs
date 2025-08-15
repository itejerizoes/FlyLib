using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.Infrastructure.Repositories
{
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        private readonly FlyLibDbContext _ctx;
        public ProvinceRepository(FlyLibDbContext ctx) : base(ctx) => _ctx = ctx;

        public Task<Province?> GetByNameAsync(string name, CancellationToken ct = default)
            => _ctx.Provinces.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name, ct);
    }
}
