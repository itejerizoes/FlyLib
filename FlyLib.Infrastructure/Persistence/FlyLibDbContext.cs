using FlyLib.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.Infrastructure.Persistence
{
    public class FlyLibDbContext : IdentityDbContext<User>
    {

        public FlyLibDbContext(DbContextOptions<FlyLibDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Visited> Visiteds { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
