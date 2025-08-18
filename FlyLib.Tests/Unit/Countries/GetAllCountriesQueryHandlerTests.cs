using FluentAssertions;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Countries.Queries.GetAllCountries;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Countries
{
    public class GetAllCountriesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllCountries()
        {
            var countries = new List<Country> { new Country("Argentina") { CountryId = 1, Iso2 = "AR" } };
            var repo = new Mock<ICountryRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetAllAsync(default)).ReturnsAsync(countries);

            // Configura el mock para mapear cada Country individualmente
            mapper.Setup(m => m.Map<CountryDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryDto(c.CountryId, c.Name, c.Iso2));

            var handler = new GetAllCountriesQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetAllCountriesQuery(), default);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Argentina");
            repo.Verify(r => r.GetAllAsync(default), Times.Once);
        }
    }
}
