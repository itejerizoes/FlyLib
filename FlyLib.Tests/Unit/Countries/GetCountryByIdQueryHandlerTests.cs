using FluentAssertions;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Countries.Queries.GetCountryById;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Countries
{
    public class GetCountryByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsCountryById()
        {
            var country = new Country("Argentina") { CountryId = 1, Iso2 = "AR" };
            var repo = new Mock<ICountryRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(country);
            mapper.Setup(m => m.Map<CountryDto>(country)).Returns(new CountryDto(1, "Argentina", "AR"));

            var handler = new GetCountryByIdQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetCountryByIdQuery(1), default);

            result.Should().NotBeNull();
            result.Name.Should().Be("Argentina");
            repo.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        }
    }
}
