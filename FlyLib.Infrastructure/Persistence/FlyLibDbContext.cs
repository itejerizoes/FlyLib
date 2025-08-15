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
        public DbSet<UserVisitedProvince> UserVisitedProvinces { get; set; }
        public DbSet<VisitPhoto> VisitPhotos { get; set; }
    }
}
