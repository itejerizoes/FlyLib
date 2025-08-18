using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Countries.Commands.CreateCountry;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Mapping;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Countries
{
    public class CreateCountryHandlerTests
    {
        [Fact]
        public async Task Creates_country_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<ICountryRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.AddAsync(It.IsAny<Country>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new CreateCountryCommandHandler(repo.Object, uow.Object, mapper);

            var result = await handler.Handle(new CreateCountryCommand("Argentina", "AR"), default);

            result.Should().BeOfType<CountryDto>();
            result.Name.Should().Be("Argentina");
            repo.Verify(r => r.AddAsync(It.IsAny<Country>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
