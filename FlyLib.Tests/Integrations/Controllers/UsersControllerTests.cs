using FluentAssertions;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
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
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
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
        public async Task GetAll_ShouldReturnUsers()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var response = await _client.GetAsync("/api/v1/users");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var users = await response.Content.ReadFromJsonAsync<List<UserResponseV1>>();
            users.Should().NotBeNull();
            users.Should().HaveCount(c => c >= 1);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserExists()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            string userId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                userId = context.Users.First().Id;
            }

            var response = await _client.GetAsync($"/api/v1/users/{userId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await response.Content.ReadFromJsonAsync<UserResponseV1>();
            user!.Id.Should().Be(userId);

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Create_ShouldAddUser()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var createRequest = new CreateUserRequestV1("nuevoUsuario", "Test");
            var response = await _client.PostAsJsonAsync("/api/v1/users", createRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<UserResponseV1>();
            created.Should().NotBeNull();
            created!.DisplayName.Should().Be("nuevoUsuario");
            created.AuthProvider.Should().Be("Test");
            string newUserId = created.Id;

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var userDb = await verifyContext.Users.FindAsync(newUserId);
                userDb.Should().NotBeNull();
                userDb!.DisplayName.Should().Be("nuevoUsuario");
                userDb.AuthProvider.Should().Be("Test");
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Update_ShouldModifyUser()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            string userId;
            string authProvider;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var user = context.Users.First();
                userId = user.Id;
                authProvider = user.AuthProvider;
            }

            var updateRequest = new UpdateUserRequestV1(userId, "Updated Name", authProvider);
            var response = await _client.PutAsJsonAsync($"/api/v1/users/{userId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/users/{userId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await getResponse.Content.ReadFromJsonAsync<UserResponseV1>();
            updated!.DisplayName.Should().Be("Updated Name");

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var userDb = await verifyContext.Users.FindAsync(userId);
                userDb!.DisplayName.Should().Be("Updated Name");
            }

            TestAuthHandler.ResetClaims();
        }

        [Fact]
        public async Task Delete_ShouldRemoveUser()
        {
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            string userId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                userId = context.Users.Last().Id;
            }

            var response = await _client.DeleteAsync($"/api/v1/users/{userId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Setea rol admin para GET
            TestAuthHandler.TestClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var getResponse = await _client.GetAsync($"/api/v1/users/{userId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var userDb = await verifyContext.Users.FindAsync(userId);
                userDb.Should().BeNull();
            }

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

            string userId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                userId = context.Users.First().Id;
            }

            var response = await _client.DeleteAsync($"/api/v1/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            TestAuthHandler.ResetClaims();
        }
    }
}
