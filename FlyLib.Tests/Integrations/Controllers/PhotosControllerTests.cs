using FluentAssertions;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Photos.Responses;
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
    public class PhotosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public PhotosControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _factory = factory;
            _client = factory.CreateClient();
        }

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
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var photoId = context.Photos.First().PhotoId;

            var response = await _client.GetAsync($"/api/v1/photos/{photoId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var photo = await response.Content.ReadFromJsonAsync<PhotoResponseV1>();
            photo!.Id.Should().Be(photoId);
        }

        [Fact]
        public async Task Update_ShouldModifyPhoto()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var photo = context.Photos.First();

            var updateRequest = new UpdatePhotoRequestV1(photo.PhotoId, "http://test/photos/updated.jpg", "Updated Photo", photo.VisitedId);
            var response = await _client.PutAsJsonAsync($"/api/v1/photos/{photo.PhotoId}", updateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/photos/{photo.PhotoId}");
            var updated = await getResponse.Content.ReadFromJsonAsync<PhotoResponseV1>();
            updated!.Description.Should().Be("Updated Photo");
        }

        [Fact]
        public async Task Delete_ShouldRemovePhoto()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
            var photoId = context.Photos.Last().PhotoId;

            var response = await _client.DeleteAsync($"/api/v1/photos/{photoId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/photos/{photoId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
