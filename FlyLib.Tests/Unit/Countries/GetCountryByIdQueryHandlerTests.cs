using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Countries.Queries.GetCountryById;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Countries
{
    public class GetCountryByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsCountryById()
        {
            var country = new Country("Argentina") { CountryId = 1, IsoCode = "AR" };
            var repo = new Mock<ICountryRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(country);
            mapper.Setup(m => m.Map<CountryDto>(country)).Returns(new CountryDto(1, "Argentina", "AR", new List<ProvinceDto>()));

            var handler = new GetCountryByIdQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetCountryByIdQuery(1), default);

            result.Should().NotBeNull();
            result.Name.Should().Be("Argentina");
            repo.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenCountryDoesNotExist()
        {
            var repo = new Mock<ICountryRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Country)null);

            var handler = new GetCountryByIdQueryHandler(repo.Object, mapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetCountryByIdQuery(99), default));
        }
    }
}
