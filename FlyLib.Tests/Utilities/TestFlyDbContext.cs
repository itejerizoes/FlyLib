using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.Tests.Utilities
{
    public class TestFlyLibDbContext : FlyLibDbContext
    {
        public TestFlyLibDbContext(DbContextOptions<FlyLibDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<Visited> Visiteds => Set<Visited>();
        public DbSet<Photo> Photos => Set<Photo>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity

            // =========================
            // Country
            // =========================
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(c => c.CountryId);
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.Iso2);

                entity.HasMany(c => c.Provinces)
                    .WithOne(p => p.Country)
                    .HasForeignKey(p => p.CountryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // Province
            // =========================
            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(p => p.ProvinceId);
                entity.Property(p => p.Name).IsRequired();

                entity.HasMany(p => p.Visiteds)
                    .WithOne(v => v.Province)
                    .HasForeignKey(v => v.ProvinceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // Visited
            // =========================
            modelBuilder.Entity<Visited>(entity =>
            {
                entity.HasKey(v => v.VisitedId);
                entity.Property(v => v.Description);
                entity.Property(v => v.VisitedAt);

                entity.HasOne(v => v.User)
                    .WithMany(u => u.Visiteds)
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(v => v.Province)
                    .WithMany(p => p.Visiteds)
                    .HasForeignKey(v => v.ProvinceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.Photos)
                    .WithOne(p => p.Visited)
                    .HasForeignKey(p => p.VisitedId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // Photo
            // =========================
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(p => p.PhotoId);
                entity.Property(p => p.Url).IsRequired();
                entity.Property(p => p.Description);
            });

            // =========================
            // User
            // =========================
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.DisplayName);
                entity.Property(u => u.AuthProvider);
            });

            // =========================
            // RefreshToken
            // =========================
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Token).IsRequired();
                entity.Property(rt => rt.ExpiresAt);

                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
