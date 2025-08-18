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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Country ===
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(c => c.CountryId);
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(c => c.Iso2)
                    .HasMaxLength(2);

                entity.HasMany(c => c.Provinces)
                    .WithOne(p => p.Country)
                    .HasForeignKey(p => p.CountryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // === Province ===
            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(p => p.ProvinceId);
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(p => p.Visiteds)
                    .WithOne(v => v.Province)
                    .HasForeignKey(v => v.ProvinceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // === Visited ===
            modelBuilder.Entity<Visited>(entity =>
            {
                entity.HasKey(v => v.VisitedId);
                entity.Property(v => v.Description)
                    .HasMaxLength(500);
                entity.Property(v => v.VisitedAt)
                    .IsRequired();

                entity.HasOne(v => v.User)
                    .WithMany(u => u.Visiteds)
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // No borrar usuarios al eliminar visitas

                entity.HasMany(v => v.Photos)
                    .WithOne(p => p.Visited)
                    .HasForeignKey(p => p.VisitedId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // === Photo ===
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(p => p.PhotoId);
                entity.Property(p => p.Url)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(p => p.Description)
                    .HasMaxLength(500);
            });

            // === User (Identity) ===
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(u => u.AuthProvider)
                    .HasMaxLength(50);

                entity.HasMany(u => u.RefreshTokens)
                    .WithOne(rt => rt.User)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // === RefreshToken ===
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(rt => rt.ExpiresAt)
                    .IsRequired();
            });
        }
    }
}
