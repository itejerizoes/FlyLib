using Azure.Storage.Blobs;
using FlyLib.API.Extensions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TestFlyLibrary.Tests.Utilities
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var inMemorySettings = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:AzureBlobStorage"] = "UseDevelopmentStorage=true" // dummy válido para tests
                };
                config.AddInMemoryCollection(inMemorySettings);
            });

            builder.ConfigureTestServices(services =>
            {
                //DbContext en memoria
                services.RemoveAll(typeof(DbContextOptions<FlyLibDbContext>));
                services.AddDbContext<FlyLibDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                //Mock de BlobServiceClient
                services.RemoveAll<BlobServiceClient>();
                var mockBlobClient = new Mock<BlobServiceClient>();
                services.AddSingleton(mockBlobClient.Object);

                //Registrar servicios de FlyLibrary usando el blob mock
                services.AddFlyLibraryServices(
                    services.BuildServiceProvider().GetRequiredService<IConfiguration>(),
                    mockBlobClient.Object
                );

                //Autenticación de prueba
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.TestScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.TestScheme;
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.TestScheme, options => { });

                //Inicialización de la base de datos
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                db.Database.EnsureCreated();

                SeedTestData(db);
            });
        }

        private void SeedTestData(FlyLibDbContext db)
        {
            if (!db.Countries.Any())
            {
                var argentina = new Country("Argentina") { CountryId = 1, Name = "Argentina", Iso2 = "AR" };
                var chile = new Country("Chile") { CountryId = 2, Name = "Chile", Iso2 = "CL" };
                var peru = new Country("Peru") { CountryId = 3, Name = "Peru", Iso2 = "PE" };
                db.Countries.AddRange(argentina, chile, peru);

                var buenosAires = new Province("Buenos Aires") { ProvinceId = 1, Name = "Buenos Aires", CountryId = argentina.CountryId };
                var cordoba = new Province("Córdoba") { ProvinceId = 2, Name = "Córdoba", CountryId = argentina.CountryId };
                var santiago = new Province("Santiago") { ProvinceId = 3, Name = "Santiago", CountryId = chile.CountryId };
                db.Provinces.AddRange(buenosAires, cordoba, santiago);

                var photo1 = new Photo("http://test/photos/1.jpg") { PhotoId = 1, Url = "http://test/photos/1.jpg", Description = "Plaza de Mayo", VisitedId = 1 };
                var photo2 = new Photo("http://test/photos/2.jpg") { PhotoId = 2, Url = "http://test/photos/2.jpg", Description = "Obelisco", VisitedId = 1 };
                var photo3 = new Photo("http://test/photos/3.jpg") { PhotoId = 3, Url = "http://test/photos/3.jpg", Description = "Córdoba Catedral", VisitedId = 2 };
                db.Photos.AddRange(photo1, photo2, photo3);

                var visited1 = new Visited(1) { VisitedId = 1, UserId = "test-user", ProvinceId = 1 };
                var visited2 = new Visited(2) { VisitedId = 2, UserId = "test-user", ProvinceId = 2 };
                db.Visiteds.AddRange(visited1, visited2);

                db.SaveChanges();
            }
        }
    }
}