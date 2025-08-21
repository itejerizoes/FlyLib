using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using System;
using System.Linq;

namespace FlyLib.Tests.Utilities
{
    public static class SeedData
    {
        public static void Initialize(FlyLibDbContext context)
        {
            if (context.Countries.Any())
                return;

            // =========================
            // Países
            // =========================
            var argentina = new Country("Argentina") { CountryId = 1, IsoCode = "ARG" };
            var chile = new Country("Chile") { CountryId = 2, IsoCode = "CHL" };
            var peru = new Country("Peru") { CountryId = 3, IsoCode = "PER" };

            context.Countries.AddRange(argentina, chile, peru);
            context.SaveChanges();

            // =========================
            // Provincias
            // =========================
            var buenosAires = new Province("Buenos Aires") { ProvinceId = 1, Country = argentina };
            var santiago = new Province("Santiago") { ProvinceId = 2, Country = chile };
            var lima = new Province("Lima") { ProvinceId = 3, Country = peru };

            context.Provinces.AddRange(buenosAires, santiago, lima);
            context.SaveChanges();

            // =========================
            // Usuarios
            // =========================
            var user1 = new User("user1") { Id = "0672044b-b448-4975-a67f-8a7b0104c122", DisplayName = "Test User", AuthProvider = "Test" };
            var user2 = new User("user2") { Id = "bbdbff39-f450-46fe-806f-9dbcf4fff41e", DisplayName = "Test User", AuthProvider = "Test" };

            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            // =========================
            // Visitas
            // =========================
            var visitedArg = new Visited(buenosAires.ProvinceId) { VisitedId = 1, Province = buenosAires, User = user1, VisitedAt = DateTime.UtcNow, Description = "Visita Argentina" };
            var visitedChi = new Visited(santiago.ProvinceId) { VisitedId = 2, Province = santiago, User = user2, VisitedAt = DateTime.UtcNow, Description = "Visita Chile" };
            var visitedPer = new Visited(lima.ProvinceId) { VisitedId = 3, Province = lima, User = user1, VisitedAt = DateTime.UtcNow, Description = "Visita Peru" };

            context.Visiteds.AddRange(visitedArg, visitedChi, visitedPer);
            context.SaveChanges();

            // =========================
            // Fotos asociadas a las visitas
            // =========================
            var photo1 = new Photo("http://example.com/photo1.jpg") { PhotoId = 1, Visited = visitedArg, Description = "Foto Argentina" };
            var photo2 = new Photo("http://example.com/photo2.jpg") { PhotoId = 2, Visited = visitedChi, Description = "Foto Chile" };
            var photo3 = new Photo("http://example.com/photo3.jpg") { PhotoId = 3, Visited = visitedPer, Description = "Foto Peru" };

            context.Photos.AddRange(photo1, photo2, photo3);
            context.SaveChanges();

            // =========================
            // RefreshTokens (opcional)
            // =========================
            var token1 = new RefreshToken { Id = 1, Token = "token1", UserId = user1.Id, ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow, IsRevoked = false };
            var token2 = new RefreshToken { Id = 2, Token = "token2", UserId = user2.Id, ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow, IsRevoked = false };

            context.RefreshTokens.AddRange(token1, token2);
            context.SaveChanges();
        }
    }
}
