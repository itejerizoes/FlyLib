using FluentAssertions;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Visited.Requests;
using FlyLib.API.DTOs.v1.Visited.Responses;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Tests.Utilities;
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
    public class VisitedsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public VisitedsControllerTests(CustomWebApplicationFactory<Program> factory)
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
        public async Task GetAll_ShouldReturnSeededVisiteds()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var response = await _client.GetAsync("/api/v1/visiteds");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var visiteds = await response.Content.ReadFromJsonAsync<List<VisitedResponseV1>>();
            visiteds.Should().NotBeNull();
            visiteds.Should().HaveCount(c => c >= 1);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task GetById_ShouldReturnVisited_WhenExists()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int visitedId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                visitedId = context.Visiteds.First().VisitedId;
            }

            var response = await _client.GetAsync($"/api/v1/visiteds/{visitedId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var visited = await response.Content.ReadFromJsonAsync<VisitedResponseV1>();
            visited!.Id.Should().Be(visitedId);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Create_ShouldAddVisited()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            string userId;
            int provinceId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                userId = context.Users.First().Id;
                provinceId = context.Provinces.First().ProvinceId;
            }

            var createRequest = new CreateVisitedRequestV1(userId, provinceId, new List<CreatePhotoRequestV1>());
            var response = await _client.PostAsJsonAsync("/api/v1/visiteds", createRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<VisitedResponseV1>();
            created.Should().NotBeNull();
            created!.UserId.Should().Be(userId);
            created.ProvinceId.Should().Be(provinceId);
            int newVisitedId = created.Id;

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var visitedDb = await verifyContext.Visiteds.FindAsync(newVisitedId);
                visitedDb.Should().NotBeNull();
                visitedDb!.UserId.Should().Be(userId);
                visitedDb.ProvinceId.Should().Be(provinceId);
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Update_ShouldModifyVisited()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int visitedId;
            string userId;
            int provinceId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var visited = context.Visiteds.First();
                visitedId = visited.VisitedId;
                userId = visited.UserId;
                provinceId = visited.ProvinceId;
            }

            var updateRequest = new UpdateVisitedRequestV1(visitedId, userId, provinceId, new List<UpdatePhotoRequestV1>());
            var response = await _client.PutAsJsonAsync($"/api/v1/visiteds/{visitedId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/visiteds/{visitedId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await getResponse.Content.ReadFromJsonAsync<VisitedResponseV1>();
            updated!.ProvinceId.Should().Be(provinceId);

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var visitedDb = await verifyContext.Visiteds.FindAsync(visitedId);
                visitedDb!.ProvinceId.Should().Be(provinceId);
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Delete_ShouldRemoveVisited()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            int visitedId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                visitedId = context.Visiteds.Last().VisitedId;
            }

            var response = await _client.DeleteAsync($"/api/v1/visiteds/{visitedId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Setea rol admin para GET
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var getResponse = await _client.GetAsync($"/api/v1/visiteds/{visitedId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var visitedDb = await verifyContext.Visiteds.FindAsync(visitedId);
                visitedDb.Should().BeNull();
            }

            TestAuthHandler.ResetClaims();
        }
    }
}
