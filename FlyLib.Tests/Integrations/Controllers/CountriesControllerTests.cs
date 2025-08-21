using FluentAssertions;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Countries.Responses;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Tests.Utilities;
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
    public class CountriesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CountriesControllerTests(CustomWebApplicationFactory<Program> factory)
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
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetAll_ShouldReturnSeededCountries()
        {
            var response = await _client.GetAsync("/api/v1/countries");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryResponseV1>>();
            countries.Should().NotBeNull();
            countries.Should().HaveCount(c => c >= 3);
            countries!.Select(c => c.Name).Should().Contain(new[] { "Argentina", "Chile", "Peru" });
        }

        [Fact]
        public async Task GetById_ShouldReturnArgentina()
        {
            int argentinaId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var argentina = context.Countries.First(c => c.Name == "Argentina");
                argentinaId = argentina.CountryId;
            }

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var response = await _client.GetAsync($"/api/v1/countries/{argentinaId}");
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
                country!.Name.Should().Be("Argentina");
                country.IsoCode.Should().Be("ARG");
            }
        }

        [Fact]
        public async Task GetByName_ShouldReturnChile()
        {
            var response = await _client.GetAsync("/api/v1/countries/byName/Chile");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var country = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            country!.Name.Should().Be("Chile");
        }

        [Fact]
        public async Task Create_ShouldAddCountry()
        {
            var createRequest = new CreateCountryRequestV1("Brasil", "BRA");
            var response = await _client.PostAsJsonAsync("/api/v1/countries", createRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            int newCountryId;
            var created = await response.Content.ReadFromJsonAsync<CountryResponseV1>();
            created.Should().NotBeNull();
            created!.Name.Should().Be("Brasil");
            created.IsoCode.Should().Be("BRA");
            newCountryId = created.CountryId;

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var brasilDb = await verifyContext.Countries.FindAsync(newCountryId);
                brasilDb.Should().NotBeNull();
                brasilDb!.Name.Should().Be("Brasil");
                brasilDb.IsoCode.Should().Be("BRA");
            }
        }

        [Fact]
        public async Task Update_ShouldModifyPeru()
        {
            int peruId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var peru = context.Countries.First(c => c.Name == "Peru");
                peruId = peru.CountryId;
            }

            var updateRequest = new UpdateCountryRequestV1(peruId, "Peru Updated", "PEU");
            var response = await _client.PutAsJsonAsync($"/api/v1/countries/{peruId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var getResponse = await _client.GetAsync($"/api/v1/countries/{peruId}");
                getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                var updated = await getResponse.Content.ReadFromJsonAsync<CountryResponseV1>();
                updated.Should().NotBeNull();
                updated!.Name.Should().Be("Peru Updated");
                updated.IsoCode.Should().Be("PEU");

                var peruDb = await verifyContext.Countries.FindAsync(peruId);
                peruDb!.Name.Should().Be("Peru Updated");
                peruDb.IsoCode.Should().Be("PEU");
            }
        }

        [Fact]
        public async Task Delete_ShouldRemoveChile()
        {
            int chileId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var chile = context.Countries.First(c => c.Name == "Chile");
                chileId = chile.CountryId;

                var response = await _client.DeleteAsync($"/api/v1/countries/{chileId}");
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var getResponse = await _client.GetAsync($"/api/v1/countries/{chileId}");
                getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

                var chileDb = await verifyContext.Countries.FindAsync(chileId);
                chileDb.Should().BeNull();
            }
        }
    }
}
