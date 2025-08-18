using FluentAssertions;
using FlyLib.Application.Visiteds.Commands.DeleteVisited;
using FlyLib.Domain.Abstractions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Visiteds
{
    public class DeleteVisitedHandlerTests
    {
        [Fact]
        public async Task Deletes_visited_ok()
        {
            var repo = new Mock<IVisitedRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.DeleteAsync(1, default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new DeleteVisitedCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new DeleteVisitedCommand(1), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.DeleteAsync(1, default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
