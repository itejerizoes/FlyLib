using FluentAssertions;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Countries.Queries.GetAllCountries;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            // Crear lista de países con provincias
            var argentina = new Country("Argentina") { CountryId = 1, IsoCode = "AR" };
            argentina.Provinces.Add(new Province("Buenos Aires") { ProvinceId = 1, CountryId = 1 });
            var countries = new List<Country> { argentina };

            var repo = new Mock<ICountryRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            // Setup usando It.IsAny para evitar conflicto con parámetros opcionales
            repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Country, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Country, object>>[]>()
            )).ReturnsAsync(countries);

            // Configura el mock para mapear cada Country a CountryDto incluyendo las Provincias
            mapper.Setup(m => m.Map<CountryDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryDto(
                    c.CountryId,
                    c.Name,
                    c.IsoCode,
                    c.Provinces
                        .Select(p => new ProvinceDto(
                            p.ProvinceId,
                            p.Name,
                            p.CountryId,
                            new List<VisitedDto>()
                        ))
                        .ToList()
                ));

            var handler = new GetAllCountriesQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetAllCountriesQuery(), default);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Argentina");
            result.First().Provinces.Should().HaveCount(1);
            result.First().Provinces.First().Name.Should().Be("Buenos Aires");

            repo.Verify(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Country, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Country, object>>[]>()
            ), Times.Once);
        }
    }
}
