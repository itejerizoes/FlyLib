using FluentAssertions;
using FlyLib.API.DTOs.v1.Provinces.Requests;
using FlyLib.API.DTOs.v1.Provinces.Responses;
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
    public class ProvincesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ProvincesControllerTests(CustomWebApplicationFactory<Program> factory)
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
        public async Task GetAll_ShouldReturnSeededProvinces()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var response = await _client.GetAsync("/api/v1/provinces");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var provinces = await response.Content.ReadFromJsonAsync<List<ProvinceResponseV1>>();
            provinces.Should().NotBeNull();
            provinces.Should().HaveCount(c => c >= 2);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task GetById_ShouldReturnSeededProvince()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int provinceId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                provinceId = context.Provinces.First().ProvinceId;
            }

            var response = await _client.GetAsync($"/api/v1/provinces/{provinceId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var province = await response.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            province!.ProvinceId.Should().Be(provinceId);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task GetByName_ShouldReturnSantiago()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var response = await _client.GetAsync("/api/v1/provinces/byName/Santiago");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var province = await response.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            province!.Name.Should().Be("Santiago");

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Create_ShouldAddProvince()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int countryId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                countryId = context.Countries.First().CountryId;
            }

            var createRequest = new CreateProvinceRequestV1("Cordoba", countryId);
            var response = await _client.PostAsJsonAsync("/api/v1/provinces", createRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            created.Should().NotBeNull();
            created!.Name.Should().Be("Cordoba");
            created.CountryId.Should().Be(countryId);
            int newProvinceId = created.ProvinceId;

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var provinceDb = await verifyContext.Provinces.FindAsync(newProvinceId);
                provinceDb.Should().NotBeNull();
                provinceDb!.Name.Should().Be("Cordoba");
                provinceDb.CountryId.Should().Be(countryId);
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Update_ShouldModifyProvince()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int provinceId;
            int countryId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var province = context.Provinces.First();
                provinceId = province.ProvinceId;
                countryId = province.CountryId;
            }

            var updateRequest = new UpdateProvinceRequestV1(provinceId, "Updated Province", countryId);
            var response = await _client.PutAsJsonAsync($"/api/v1/provinces/{provinceId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/provinces/{provinceId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await getResponse.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            updated!.Name.Should().Be("Updated Province");

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var provinceDb = await verifyContext.Provinces.FindAsync(provinceId);
                provinceDb!.Name.Should().Be("Updated Province");
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Delete_ShouldRemoveProvince()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int provinceId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                provinceId = context.Provinces.Last().ProvinceId;
            }

            var response = await _client.DeleteAsync($"/api/v1/provinces/{provinceId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Setea rol admin para GET
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var getResponse = await _client.GetAsync($"/api/v1/provinces/{provinceId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var provinceDb = await verifyContext.Provinces.FindAsync(provinceId);
                provinceDb.Should().BeNull();
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

            int countryId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                countryId = context.Countries.First().CountryId;
            }

            var createRequest = new CreateProvinceRequestV1("La Pampa", countryId);
            var response = await _client.PostAsJsonAsync("/api/v1/provinces", createRequest);

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

            int provinceId;
            int countryId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var province = context.Provinces.First();
                provinceId = province.ProvinceId;
                countryId = province.CountryId;
            }

            var updateRequest = new UpdateProvinceRequestV1(provinceId, "La Pampa", countryId);
            var response = await _client.PutAsJsonAsync($"/api/v1/provinces/{provinceId}", updateRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Delete_ShouldReturnForbidden_WhenNotAdmin()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User")
            };

            int provinceId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                provinceId = context.Provinces.First().ProvinceId;
            }

            var response = await _client.DeleteAsync($"/api/v1/provinces/{provinceId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            TestAuthHandler.ResetClaims();
        }
    }
}