using FluentAssertions;
using FlyLib.API.DTOs.v1.Provinces.Requests;
using FlyLib.API.DTOs.v1.Provinces.Responses;
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
    public class ProvincesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ProvincesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededProvinces()
        {
            var response = await _client.GetAsync("/api/v1/provinces");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var provinces = await response.Content.ReadFromJsonAsync<List<ProvinceResponseV1>>();
            provinces.Should().NotBeNull();
            provinces.Should().HaveCount(c => c >= 2);
        }

        [Fact]
        public async Task GetById_ShouldReturnSeededProvince()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var provinceId = context.Provinces.First().ProvinceId;

            var response = await _client.GetAsync($"/api/v1/provinces/{provinceId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var province = await response.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            province!.ProvinceId.Should().Be(provinceId);
        }

        [Fact]
        public async Task Update_ShouldModifyProvince()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var province = context.Provinces.First();

            var updateRequest = new UpdateProvinceRequestV1(province.ProvinceId, "Updated Province", province.CountryId);
            var response = await _client.PutAsJsonAsync($"/api/v1/provinces/{province.ProvinceId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/provinces/{province.ProvinceId}");
            var updated = await getResponse.Content.ReadFromJsonAsync<ProvinceResponseV1>();
            updated!.Name.Should().Be("Updated Province");
        }

        [Fact]
        public async Task Delete_ShouldRemoveProvince()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var provinceId = context.Provinces.Last().ProvinceId;

            var response = await _client.DeleteAsync($"/api/v1/provinces/{provinceId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/provinces/{provinceId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
