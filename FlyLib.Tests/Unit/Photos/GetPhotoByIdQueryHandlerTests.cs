using FluentAssertions;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Application.Photos.Queries.GetPhotoById;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Photos
{
    public class GetPhotoByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsPhotoById()
        {
            var photo = new Photo("http://url.com/foto.jpg") { PhotoId = 1, VisitedId = 1, Description = "desc" };
            var repo = new Mock<IPhotoRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(photo);
            mapper.Setup(m => m.Map<PhotoDto>(photo)).Returns(new PhotoDto(1, "http://url.com/foto.jpg", "desc", 1));

            var handler = new GetPhotoByIdQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetPhotoByIdQuery(1), default);

            result.Should().NotBeNull();
            result.Url.Should().Be("http://url.com/foto.jpg");
            repo.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        }
    }
}
