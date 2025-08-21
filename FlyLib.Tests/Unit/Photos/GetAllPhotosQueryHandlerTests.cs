using FluentAssertions;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Application.Photos.Queries.GetAllPhotos;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Photos
{
    public class GetAllPhotosQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllPhotos()
        {
            var photos = new List<Photo> { new Photo("http://url.com/foto.jpg") { PhotoId = 1 } };
            var repo = new Mock<IPhotoRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Photo, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Photo, object>>[]>()
            )).ReturnsAsync(photos);
            mapper.Setup(m => m.Map<PhotoDto>(It.IsAny<Photo>()))
                .Returns((Photo p) => new PhotoDto(p.PhotoId, p.Url, p.Description, p.VisitedId));

            var handler = new GetAllPhotosQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetAllPhotosQuery(), default);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Url.Should().Be("http://url.com/foto.jpg");
            repo.Verify(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Photo, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Photo, object>>[]>()
            ), Times.Once);
        }
    }
}
