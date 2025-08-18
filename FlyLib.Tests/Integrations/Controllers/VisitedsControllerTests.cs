using FluentAssertions;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Visited.Requests;
using FlyLib.API.DTOs.v1.Visited.Responses;
using FlyLib.Domain.Entities;
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
    public class VisitedsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public VisitedsControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededVisiteds()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/visiteds");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var visiteds = await response.Content.ReadFromJsonAsync<List<VisitedResponseV1>>();
            visiteds.Should().NotBeNull();
            visiteds.Should().HaveCount(c => c >= 2);
            visiteds!.Select(v => v.ProvinceId).Should().Contain(new[] { 1, 2 });
        }

        [Fact]
        public async Task GetById_ShouldReturnVisited_WhenExists()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/visiteds/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var visited = await response.Content.ReadFromJsonAsync<VisitedResponseV1>();
            visited!.Id.Should().Be(1);
            visited.UserId.Should().Be("test-user");
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedVisited()
        {
            // Arrange
            var request = new CreateCountryRequestV1("Test Visit", "TE");

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/visiteds", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<VisitedResponseV1>();
            created.Should().NotBeNull();
            created!.Id.Should().BePositive();
        }

        [Fact]
        public async Task Update_ShouldModifyVisited()
        {
            // Arrange
            var updateRequest = new UpdateVisitedRequestV1(1, "test-user", 3, new List<Photo>());

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/visiteds/1", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/visiteds/1");
            var updated = await getResponse.Content.ReadFromJsonAsync<VisitedResponseV1>();
            updated!.ProvinceId.Should().Be(3);
        }

        [Fact]
        public async Task Delete_ShouldRemoveVisited()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/visiteds/2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/visiteds/2");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
