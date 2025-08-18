using FluentAssertions;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Countries.Responses;
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
    public class CountriesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CountriesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededCountries()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/countries");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryResponseV1>>();
            countries.Should().NotBeNull();
            countries.Should().HaveCount(c => c >= 3);
            countries!.Select(c => c.Name).Should().Contain(new[] { "Argentina", "Chile", "Peru" });
        }

        [Fact]
        public async Task GetById_ShouldReturnArgentina()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/countries/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            country!.Name.Should().Be("Argentina");
            country.IsoCode.Should().Be("AR");
        }

        [Fact]
        public async Task GetByName_ShouldReturnChile()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/countries/byName/Chile");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            country!.Name.Should().Be("Chile");
        }

        [Fact]
        public async Task Update_ShouldModifyPeru()
        {
            // Arrange
            var updateRequest = new UpdateCountryRequestV1(3, "Peru Updated", "PEU");

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/countries/3", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/countries/3");
            var updated = await getResponse.Content.ReadFromJsonAsync<CountryResponseV1>();
            updated!.Name.Should().Be("Peru Updated");
            updated.IsoCode.Should().Be("PEU");
        }

        [Fact]
        public async Task Delete_ShouldRemoveChile()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/countries/2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/countries/2");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
