using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Photos.Commands.DeletePhoto;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Photos
{
    public class DeletePhotoHandlerTests
    {
        [Fact]
        public async Task Deletes_photo_ok()
        {
            var repo = new Mock<IPhotoRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(new Photo("url"));
            repo.Setup(r => r.DeleteAsync(1, default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new DeletePhotoCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new DeletePhotoCommand(1), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.DeleteAsync(1, default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenPhotoDoesNotExist()
        {
            var repo = new Mock<IPhotoRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Photo)null);

            var handler = new DeletePhotoCommandHandler(repo.Object, uow.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new DeletePhotoCommand(99), default));
        }
    }
}
