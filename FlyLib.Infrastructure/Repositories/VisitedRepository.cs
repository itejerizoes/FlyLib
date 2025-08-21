using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class VisitedRepository : Repository<Visited>, IVisitedRepository
    {
        public VisitedRepository(FlyLibDbContext context) : base(context) { }
    }
}
