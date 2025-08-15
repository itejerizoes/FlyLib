using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class VisitPhotoRepository : Repository<VisitPhoto>, IVisitPhotoRepository
    {
        private readonly FlyLibDbContext _ctx;
        public VisitPhotoRepository(FlyLibDbContext ctx) : base(ctx) => _ctx = ctx;
    }
}
