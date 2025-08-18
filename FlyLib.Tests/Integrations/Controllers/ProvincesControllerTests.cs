using FluentAssertions;
using FlyLib.API.DTOs.v1.Provinces.Requests;
using FlyLib.API.DTOs.v1.Provinces.Responses;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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
    public class ProvincesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProvincesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededProvinces()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/provinces");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var provinces = await response.Content.ReadFromJsonAsync<List<ProvinceResponseV1>>();
            provinces.Should().NotBeNull();
            provinces.Should().HaveCount(c => c >= 3);
            provinces.Select(p => p.Name).Should().Contain(new[] { "Buenos Aires", "Córdoba", "Santiago" });
        }

        [Fact]
        public async Task GetById_ShouldReturnBuenosAires()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/provinces/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var province = await response.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            province!.Name.Should().Be("Buenos Aires");
        }

        [Fact]
        public async Task GetByName_ShouldReturnCordoba()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/provinces/byName/Córdoba");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var province = await response.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            province!.Name.Should().Be("Córdoba");
        }

        [Fact]
        public async Task Update_ShouldModifyProvince()
        {
            // Arrange
            var updateRequest = new UpdateProvinceRequestV1(3, "Santiago Updated", 2);

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/provinces/3", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/provinces/3");
            var updated = await getResponse.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            updated!.Name.Should().Be("Santiago Updated");
        }

        [Fact]
        public async Task Delete_ShouldRemoveProvince()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/provinces/2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/provinces/2");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
