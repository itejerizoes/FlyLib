using FluentAssertions;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Visited.Requests;
using FlyLib.API.DTOs.v1.Visited.Responses;
using FlyLib.Domain.Entities;
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
    public class VisitedsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public VisitedsControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededVisiteds()
        {
            var response = await _client.GetAsync("/api/v1/visiteds");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var visiteds = await response.Content.ReadFromJsonAsync<List<VisitedResponseV1>>();
            visiteds.Should().NotBeNull();
            visiteds.Should().HaveCount(c => c >= 1);
        }

        [Fact]
        public async Task GetById_ShouldReturnVisited_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var visitedId = context.Visiteds.First().VisitedId;

            var response = await _client.GetAsync($"/api/v1/visiteds/{visitedId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var visited = await response.Content.ReadFromJsonAsync<VisitedResponseV1>();
            visited!.Id.Should().Be(visitedId);
        }

        [Fact]
        public async Task Update_ShouldModifyVisited()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var visited = context.Visiteds.First();

            var updateRequest = new UpdateVisitedRequestV1(visited.VisitedId, visited.UserId, visited.ProvinceId, new List<Photo>());
            var response = await _client.PutAsJsonAsync($"/api/v1/visiteds/{visited.VisitedId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/visiteds/{visited.VisitedId}");
            var updated = await getResponse.Content.ReadFromJsonAsync<VisitedResponseV1>();
            updated!.ProvinceId.Should().Be(visited.ProvinceId);
        }

        [Fact]
        public async Task Delete_ShouldRemoveVisited()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var visitedId = context.Visiteds.Last().VisitedId;

            var response = await _client.DeleteAsync($"/api/v1/visiteds/{visitedId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/visiteds/{visitedId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
