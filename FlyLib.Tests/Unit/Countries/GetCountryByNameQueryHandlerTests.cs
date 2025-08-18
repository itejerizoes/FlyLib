using FluentAssertions;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Countries.Queries.GetCountryByName;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Countries
{
    public class GetCountryByNameQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsCountryByName()
        {
            var country = new Country("Argentina") { CountryId = 1, Iso2 = "AR" };
            var repo = new Mock<ICountryRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByNameAsync("Argentina", default)).ReturnsAsync(country);
            mapper.Setup(m => m.Map<CountryDto>(country)).Returns(new CountryDto(1, "Argentina", "AR"));

            var handler = new GetCountryByNameQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetCountryByNameQuery("Argentina"), default);

            result.Should().NotBeNull();
            result.Name.Should().Be("Argentina");
            repo.Verify(r => r.GetByNameAsync("Argentina", default), Times.Once);
        }
    }
}
