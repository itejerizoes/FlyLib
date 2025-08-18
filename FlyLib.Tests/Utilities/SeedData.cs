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
                return; // Ya hay datos

            // =========================
            // Crear países
            // =========================
            var argentina = new Country("Argentina");
            var chile = new Country("Chile");
            var peru = new Country("Peru");

            context.Countries.AddRange(argentina, chile, peru);
            context.SaveChanges(); // <- Genera CountryId

            // =========================
            // Crear provincias
            // =========================
            var buenosAires = new Province("Buenos Aires") { Country = argentina };
            var santiago = new Province("Santiago") { Country = chile };
            var lima = new Province("Lima") { Country = peru };

            context.Provinces.AddRange(buenosAires, santiago, lima);
            context.SaveChanges(); // <- Genera ProvinceId

            // =========================
            // Crear usuario de prueba
            // =========================
            var user = new User("testuser")
            {
                DisplayName = "Test User",
                AuthProvider = "Test"
            };
            context.Users.Add(user);
            context.SaveChanges(); // <- Genera UserId

            // =========================
            // Crear visita vinculada a la provincia y al usuario
            // =========================
            var visited = new Visited(santiago.ProvinceId)
            {
                Province = santiago,
                User = user,
                VisitedAt = DateTime.UtcNow,
                Description = "Visita de prueba"
            };
            context.Visiteds.Add(visited);
            context.SaveChanges(); // <- Genera VisitedId

            // =========================
            // Crear fotos asociadas a la visita
            // =========================
            var photo1 = new Photo("http://example.com/photo1.jpg") { Visited = visited };
            var photo2 = new Photo("http://example.com/photo2.jpg") { Visited = visited };

            context.Photos.AddRange(photo1, photo2);
            context.SaveChanges();
        }
    }
}
