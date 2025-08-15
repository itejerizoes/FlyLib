using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly FlyLibDbContext _ctx;
        public UserRepository(FlyLibDbContext ctx) : base(ctx) => _ctx = ctx;
    }
}
