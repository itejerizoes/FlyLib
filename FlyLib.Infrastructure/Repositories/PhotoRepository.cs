using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(FlyLibDbContext context) : base(context) { }
    }
}
