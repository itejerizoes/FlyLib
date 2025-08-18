using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        private readonly FlyLibDbContext _ctx;
        public PhotoRepository(FlyLibDbContext ctx) : base(ctx) => _ctx = ctx;
    }
}
