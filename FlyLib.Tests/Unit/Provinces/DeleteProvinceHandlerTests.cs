using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Provinces.Commands.DeleteProvince;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Provinces
{
    public class DeleteProvinceHandlerTests
    {
        [Fact]
        public async Task Deletes_province_ok()
        {
            var repo = new Mock<IProvinceRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(new Province("Santiago"));
            repo.Setup(r => r.DeleteAsync(1, default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new DeleteProvinceCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new DeleteProvinceCommand(1), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.DeleteAsync(1, default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenProvinceDoesNotExist()
        {
            var repo = new Mock<IProvinceRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Province)null);

            var handler = new DeleteProvinceCommandHandler(repo.Object, uow.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new DeleteProvinceCommand(99), default));
        }
    }
}
