using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;

namespace FlyLib.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FlyLibDbContext context) : base(context) { }
    }
}
