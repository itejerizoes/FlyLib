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
            var argentina = new Country("Argentina");
            var chile = new Country("Chile");
            var peru = new Country("Peru");

            context.Countries.AddRange(argentina, chile, peru);
            context.SaveChanges();

            // =========================
            // Provincias (solo Argentina y Perú, no Chile)
            // =========================
            var buenosAires = new Province("Buenos Aires") { Country = argentina };
            var lima = new Province("Lima") { Country = peru };

            context.Provinces.AddRange(buenosAires, lima);
            context.SaveChanges();

            // =========================
            // Usuario de prueba
            // =========================
            var user = new User("testuser")
            {
                DisplayName = "Test User",
                AuthProvider = "Test"
            };
            context.Users.Add(user);
            context.SaveChanges();

            // =========================
            // Visita (solo en Perú)
            // =========================
            var visited = new Visited(lima.ProvinceId)
            {
                Province = lima,
                User = user,
                VisitedAt = DateTime.UtcNow,
                Description = "Visita de prueba"
            };
            context.Visiteds.Add(visited);
            context.SaveChanges();

            // =========================
            // Fotos asociadas a la visita
            // =========================
            var photo1 = new Photo("http://example.com/photo1.jpg") { Visited = visited };
            var photo2 = new Photo("http://example.com/photo2.jpg") { Visited = visited };

            context.Photos.AddRange(photo1, photo2);
            context.SaveChanges();
        }
    }
}
