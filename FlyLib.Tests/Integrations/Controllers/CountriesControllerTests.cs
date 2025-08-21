using FluentAssertions;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Countries.Responses;
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
    public class CountriesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CountriesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededCountries()
        {
            var response = await _client.GetAsync("/api/v1/countries");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Console.WriteLine(response);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryResponseV1>>();
            Console.WriteLine(response);
            countries.Should().NotBeNull();
            countries.Should().HaveCount(c => c >= 3);
            countries!.Select(c => c.Name).Should().Contain(new[] { "Argentina", "Chile", "Peru" });
        }

        [Fact]
        public async Task GetById_ShouldReturnArgentina()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var argentina = context.Countries.First(c => c.Name == "Argentina");

            var response = await _client.GetAsync($"/api/v1/countries/{argentina.CountryId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Console.WriteLine(response);

            var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            Console.WriteLine(country);
            country!.Name.Should().Be("Argentina");
            country.IsoCode.Should().Be("AR");
        }

        [Fact]
        public async Task GetByName_ShouldReturnChile()
        {
            var response = await _client.GetAsync("/api/v1/countries/byName/Chile");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Console.WriteLine(response);

            var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            country!.Name.Should().Be("Chile");
            Console.WriteLine(country);
        }

        [Fact]
        public async Task Update_ShouldModifyPeru()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var peru = context.Countries.First(c => c.Name == "Peru");

            var updateRequest = new UpdateCountryRequestV1(peru.CountryId, "Peru Updated", "PEU");

            var response = await _client.PutAsJsonAsync($"/api/v1/countries/{peru.CountryId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            Console.WriteLine(response);

            var getResponse = await _client.GetAsync($"/api/v1/countries/{peru.CountryId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            Console.WriteLine(getResponse);

            var updated = await getResponse.Content.ReadFromJsonAsync<CountryResponseV1>();
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Peru Updated");
            updated.IsoCode.Should().Be("PEU");

            // Validación directa en la DB
            var peruDb = await context.Countries.FindAsync(peru.CountryId);
            Console.WriteLine(peruDb);
            peruDb!.Name.Should().Be("Peru Updated");
            peruDb.IsoCode.Should().Be("PEU");
        }

        [Fact]
        public async Task Delete_ShouldRemoveChile()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var chile = context.Countries.First(c => c.Name == "Chile");

            // Ejecutar DELETE
            var response = await _client.DeleteAsync($"/api/v1/countries/{chile.CountryId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            Console.WriteLine(response);

            // GET después de DELETE debería ser 404
            var getResponse = await _client.GetAsync($"/api/v1/countries/{chile.CountryId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Console.WriteLine(getResponse);

            // Validación directa en la DB
            var chileDb = await context.Countries.FindAsync(chile.CountryId);
            chileDb.Should().BeNull(); // Esto ahora debería pasar
            Console.WriteLine(chileDb);
        }
    }
}
