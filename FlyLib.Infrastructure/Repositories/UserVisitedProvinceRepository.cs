using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class UserVisitedProvinceRepository : Repository<UserVisitedProvince>, IUserVisitedProvinceRepository
    {
        private readonly FlyLibDbContext _ctx;
        public UserVisitedProvinceRepository(FlyLibDbContext ctx) : base(ctx) => _ctx = ctx;
    }
}
