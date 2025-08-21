using FluentAssertions;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
using FlyLib.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestFlyLibrary.Tests.Utilities;
using Xunit;

namespace FlyLib.Tests.Integrations.Controllers
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnUsers()
        {
            var response = await _client.GetAsync("/api/v1/users");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var users = await response.Content.ReadFromJsonAsync<List<UserResponseV1>>();
            users.Should().NotBeNull();
            users.Should().HaveCount(c => c >= 1);
        }

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserExists()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var userId = context.Users.First().Id;

            var response = await _client.GetAsync($"/api/v1/users/{userId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await response.Content.ReadFromJsonAsync<UserResponseV1>();
            user!.Id.Should().Be(userId);
        }

        [Fact]
        public async Task Update_ShouldModifyUser()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var user = context.Users.First();

            var updateRequest = new UpdateUserRequestV1(user.Id, "Updated Name", user.AuthProvider);
            var response = await _client.PutAsJsonAsync($"/api/v1/users/{user.Id}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/users/{user.Id}");
            var updated = await getResponse.Content.ReadFromJsonAsync<UserResponseV1>();
            updated!.DisplayName.Should().Be("Updated Name");
        }

        [Fact]
        public async Task Delete_ShouldRemoveUser()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var userId = context.Users.Last().Id;

            var response = await _client.DeleteAsync($"/api/v1/users/{userId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/users/{userId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
