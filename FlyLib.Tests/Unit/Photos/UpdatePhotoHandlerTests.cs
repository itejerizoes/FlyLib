using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Photos.Commands.UpdatePhoto;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Photos
{
    public class UpdatePhotoHandlerTests
    {
        [Fact]
        public async Task Updates_photo_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<IPhotoRepository>();
            var uow = new Mock<IUnitOfWork>();

            var photo = new Photo("http://url.com/foto.jpg") { PhotoId = 1, VisitedId = 1 };
            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(photo);
            repo.Setup(r => r.UpdateAsync(It.IsAny<Photo>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new UpdatePhotoCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new UpdatePhotoCommand(1, "http://url.com/foto.jpg", "Descripción", 1), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.UpdateAsync(It.IsAny<Photo>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenPhotoDoesNotExist()
        {
            var repo = new Mock<IPhotoRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Photo)null);

            var handler = new UpdatePhotoCommandHandler(repo.Object, uow.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new UpdatePhotoCommand(99, "url", "desc", 1), default));
        }
    }
}
