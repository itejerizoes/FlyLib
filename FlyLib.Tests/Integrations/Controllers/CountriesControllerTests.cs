using FluentAssertions;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Countries.Responses;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Tests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using TestFlyLibrary.Tests.Utilities;
using Xunit;

namespace FlyLib.Tests.Integrations.Controllers
{
    public class CountriesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CountriesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _factory = factory;
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            SeedData.Initialize(db);
            TestAuthHandler.ResetClaims();
        }

        public Task DisposeAsync()
        {
            TestAuthHandler.ResetClaims();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededCountries()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var response = await _client.GetAsync("/api/v1/countries");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryResponseV1>>();
            countries.Should().NotBeNull();
            countries.Should().HaveCount(c => c >= 3);
            countries!.Select(c => c.Name).Should().Contain(new[] { "Argentina", "Chile", "Peru" });

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task GetById_ShouldReturnArgentina()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int argentinaId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var argentina = context.Countries.First(c => c.Name == "Argentina");
                argentinaId = argentina.CountryId;
            }

            var response = await _client.GetAsync($"/api/v1/countries/{argentinaId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            country!.Name.Should().Be("Argentina");
            country.IsoCode.Should().Be("ARG");

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task GetByName_ShouldReturnChile()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var response = await _client.GetAsync("/api/v1/countries/byName/Chile");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            country!.Name.Should().Be("Chile");

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Create_ShouldAddCountry()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var createRequest = new CreateCountryRequestV1("Brasil", "BRA");
            var response = await _client.PostAsJsonAsync("/api/v1/countries", createRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            created.Should().NotBeNull();
            created!.Name.Should().Be("Brasil");
            created.IsoCode.Should().Be("BRA");
            int newCountryId = created.CountryId;

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var brasilDb = await verifyContext.Countries.FindAsync(newCountryId);
                brasilDb.Should().NotBeNull();
                brasilDb!.Name.Should().Be("Brasil");
                brasilDb.IsoCode.Should().Be("BRA");
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Update_ShouldModifyPeru()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int peruId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var peru = context.Countries.First(c => c.Name == "Peru");
                peruId = peru.CountryId;
            }

            var updateRequest = new UpdateCountryRequestV1(peruId, "Peru Updated", "PEU");
            var response = await _client.PutAsJsonAsync($"/api/v1/countries/{peruId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/countries/{peruId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await getResponse.Content.ReadFromJsonAsync<CountryResponseV1>();
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Peru Updated");
            updated.IsoCode.Should().Be("PEU");

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var peruDb = await verifyContext.Countries.FindAsync(peruId);
                peruDb!.Name.Should().Be("Peru Updated");
                peruDb.IsoCode.Should().Be("PEU");
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Delete_ShouldRemoveChile()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int chileId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var chile = context.Countries.First(c => c.Name == "Chile");
                chileId = chile.CountryId;
            }

            var response = await _client.DeleteAsync($"/api/v1/countries/{chileId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Setea rol admin para GET
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var getResponse = await _client.GetAsync($"/api/v1/countries/{chileId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var chileDb = await verifyContext.Countries.FindAsync(chileId);
                chileDb.Should().BeNull();
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Create_ShouldReturnForbidden_WhenNotAdmin()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User")
            };

            var createRequest = new CreateCountryRequestV1("Uruguay", "URU");
            var response = await _client.PostAsJsonAsync("/api/v1/countries", createRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Update_ShouldReturnForbidden_WhenNotAdmin()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User")
            };

            int countryId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                countryId = context.Countries.First().CountryId;
            }

            var updateRequest = new UpdateCountryRequestV1(countryId, "Uruguay", "URU");
            var response = await _client.PutAsJsonAsync($"/api/v1/countries/{countryId}", updateRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Delete_ShouldReturnForbidden_WhenNotAdmin()
        {
            // Setea rol User antes de la petición
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User")
            };

            int countryId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                countryId = context.Countries.First().CountryId;
            }

            var response = await _client.DeleteAsync($"/api/v1/countries/{countryId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            TestAuthHandler.ResetClaims();
        }
    }
}
