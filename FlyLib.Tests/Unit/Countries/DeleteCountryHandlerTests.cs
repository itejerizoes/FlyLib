using FluentAssertions;
using FlyLib.Application.Countries.Commands.DeleteCountry;
using FlyLib.Domain.Abstractions;
using MediatR;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Countries
{
    public class DeleteCountryHandlerTests
    {
        [Fact]
        public async Task Deletes_country_ok()
        {
            var repo = new Mock<ICountryRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.DeleteAsync(1, default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new DeleteCountryCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new DeleteCountryCommand(1), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.DeleteAsync(1, default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
