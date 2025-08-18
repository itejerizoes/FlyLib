using FluentAssertions;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Photos.Responses;
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

        public PhotosControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSeededPhotos()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/photos");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var photos = await response.Content.ReadFromJsonAsync<List<PhotoResponseV1>>();
            photos.Should().NotBeNull();
            photos.Should().HaveCount(c => c >= 3);
            photos!.Select(p => p.Description).Should().Contain(new[] { "Plaza de Mayo", "Obelisco", "Cordoba Catedral" });
        }

        [Fact]
        public async Task GetById_ShouldReturnPlazaDeMayo()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/photos/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var photo = await response.Content.ReadFromJsonAsync<PhotoResponseV1>();
            photo!.Description.Should().Be("Plaza de Mayo");
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedPhoto()
        {
            // Arrange
            var request = new CreatePhotoRequestV1("http://test/photos/4.jpg", "Nueva Foto");

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/photos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<PhotoResponseV1>();
            created.Should().NotBeNull();
            created!.Description.Should().Be("Nueva Foto");
        }

        [Fact]
        public async Task Update_ShouldModifyPhoto()
        {
            // Arrange
            var updateRequest = new UpdatePhotoRequestV1(2, "http://test/photos/2_updated.jpg", "Obelisco Updated", 1);

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/photos/2", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/photos/2");
            var updated = await getResponse.Content.ReadFromJsonAsync<PhotoResponseV1>();
            updated!.Description.Should().Be("Obelisco Updated");
        }

        [Fact]
        public async Task Delete_ShouldRemovePhoto()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/photos/3");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync("/api/v1/photos/3");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
