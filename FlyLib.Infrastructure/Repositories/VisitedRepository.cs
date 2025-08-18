using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class VisitedRepository : Repository<Visited>, IVisitedRepository
    {
        private readonly FlyLibDbContext _ctx;
        public VisitedRepository(FlyLibDbContext ctx) : base(ctx) => _ctx = ctx;
    }
}
