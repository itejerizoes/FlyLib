using FluentAssertions;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Photos.Responses;
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
    public class PhotosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public PhotosControllerTests(CustomWebApplicationFactory<Program> factory)
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
        public async Task GetAll_ShouldReturnSeededPhotos()
        {
            var response = await _client.GetAsync("/api/v1/photos");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var photos = await response.Content.ReadFromJsonAsync<List<PhotoResponseV1>>();
            photos.Should().NotBeNull();
            photos.Should().HaveCount(c => c >= 2);
        }

        [Fact]
        public async Task GetById_ShouldReturnSeededPhoto()
        {
            int photoId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                photoId = context.Photos.First().PhotoId;
            }

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var response = await _client.GetAsync($"/api/v1/photos/{photoId}");
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var photo = await response.Content.ReadFromJsonAsync<PhotoResponseV1>();
                photo!.Id.Should().Be(photoId);
            }
        }

        [Fact]
        public async Task Create_ShouldAddPhoto()
        {
            int visitedId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                visitedId = context.Visiteds.First().VisitedId;
            }

            var createRequest = new CreatePhotoRequestV1("http://test/photos/new.jpg", "Nueva Foto", visitedId);
            var response = await _client.PostAsJsonAsync("/api/v1/photos", createRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<PhotoResponseV1>();
            created.Should().NotBeNull();
            created!.Url.Should().Be("http://test/photos/new.jpg");
            created.Description.Should().Be("Nueva Foto");
            created.VisitedId.Should().Be(visitedId);

            int newPhotoId = created.Id;

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var photoDb = await verifyContext.Photos.FindAsync(newPhotoId);
                photoDb.Should().NotBeNull();
                photoDb!.Url.Should().Be("http://test/photos/new.jpg");
                photoDb.Description.Should().Be("Nueva Foto");
                photoDb.VisitedId.Should().Be(visitedId);
            }
        }

        [Fact]
        public async Task Update_ShouldModifyPhoto()
        {
            int photoId;
            int visitedId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var photo = context.Photos.First();
                photoId = photo.PhotoId;
                visitedId = photo.VisitedId;
            }

            var updateRequest = new UpdatePhotoRequestV1(photoId, "http://test/photos/updated.jpg", "Updated Photo", visitedId);
            var response = await _client.PutAsJsonAsync($"/api/v1/photos/{photoId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var getResponse = await _client.GetAsync($"/api/v1/photos/{photoId}");
                getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                var updated = await getResponse.Content.ReadFromJsonAsync<PhotoResponseV1>();
                updated!.Description.Should().Be("Updated Photo");

                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var photoDb = await verifyContext.Photos.FindAsync(photoId);
                photoDb!.Description.Should().Be("Updated Photo");
                photoDb.Url.Should().Be("http://test/photos/updated.jpg");
            }
        }

        [Fact]
        public async Task Delete_ShouldRemovePhoto()
        {
            int photoId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                photoId = context.Photos.Last().PhotoId;

                var response = await _client.DeleteAsync($"/api/v1/photos/{photoId}");
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }

            using (var verifyScope = _factory.Services.CreateScope())
            {
                var getResponse = await _client.GetAsync($"/api/v1/photos/{photoId}");
                getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

                var verifyContext = verifyScope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
                var photoDb = await verifyContext.Photos.FindAsync(photoId);
                photoDb.Should().BeNull();
            }
        }
    }
}
