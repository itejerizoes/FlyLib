using FluentAssertions;
using FlyLib.Application.Users.Commands.DeleteUser;
using FlyLib.Domain.Abstractions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Users
{
    public class DeleteUserHandlerTests
    {
        [Fact]
        public async Task Deletes_user_ok()
        {
            var repo = new Mock<IUserRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.DeleteAsync("1", default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new DeleteUserCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new DeleteUserCommand("1"), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.DeleteAsync("1", default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
