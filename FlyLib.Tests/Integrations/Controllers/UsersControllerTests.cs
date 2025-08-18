using FluentAssertions;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
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

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnUsers()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var users = await response.Content.ReadFromJsonAsync<List<UserResponseV1>>();
            users.Should().NotBeNull();
            users.Should().HaveCount(c => c >= 1); // Aseguramos que haya al menos un usuario seed
        }

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var testUserId = "test-user"; // Usando el Id del usuario autenticado en TestAuthHandler

            // Act
            var response = await _client.GetAsync($"/api/v1/users/{testUserId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await response.Content.ReadFromJsonAsync<UserResponseV1>();
            user.Should().NotBeNull();
            user!.Id.Should().Be(testUserId);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedUser()
        {
            // Arrange
            var request = new CreateUserRequestV1("Nuevo Usuario", "nuevo@example.com");

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/users", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<UserResponseV1>();
            created.Should().NotBeNull();
            created!.DisplayName.Should().Be("Nuevo Usuario");
            created.AuthProvider.Should().Be("nuevo@example.com");
        }

        [Fact]
        public async Task Update_ShouldModifyUser()
        {
            // Arrange
            var updateRequest = new UpdateUserRequestV1("test-user", "Usuario Actualizado", "actualizado@example.com");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/users/{updateRequest.Id}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/users/{updateRequest.Id}");
            var updated = await getResponse.Content.ReadFromJsonAsync<UserResponseV1>();
            updated!.DisplayName.Should().Be("Usuario Actualizado");
            updated.AuthProvider.Should().Be("actualizado@example.com");
        }

        [Fact]
        public async Task Delete_ShouldRemoveUser()
        {
            // Arrange
            var userIdToDelete = "test-user";

            // Act
            var response = await _client.DeleteAsync($"/api/v1/users/{userIdToDelete}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/users/{userIdToDelete}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
