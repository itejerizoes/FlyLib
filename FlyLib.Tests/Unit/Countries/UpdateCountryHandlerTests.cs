using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Countries.Commands.UpdateCountry;
using FlyLib.Application.Mapping;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Countries
{
    public class UpdateCountryHandlerTests
    {
        [Fact]
        public async Task Updates_country_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<ICountryRepository>();
            var uow = new Mock<IUnitOfWork>();

            var country = new Country("Argentina") { CountryId = 1, IsoCode = "AR" };
            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(country);
            repo.Setup(r => r.UpdateAsync(It.IsAny<Country>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new UpdateCountryCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new UpdateCountryCommand(1, "Argentina", "AR"), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.UpdateAsync(It.IsAny<Country>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenCountryDoesNotExist()
        {
            var repo = new Mock<ICountryRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Country)null);

            var handler = new UpdateCountryCommandHandler(repo.Object, uow.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new UpdateCountryCommand(99, "NoExiste", "XX"), default));
        }
    }
}
