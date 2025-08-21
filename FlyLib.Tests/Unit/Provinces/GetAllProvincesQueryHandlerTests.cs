using FluentAssertions;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Application.Provinces.Queries.GetAllProvinces;
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

namespace FlyLib.Tests.Unit.Provinces
{
    public class GetAllProvincesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllProvinces()
        {
            // Arrange
            var provinces = new List<Province>
            {
                new Province("Buenos Aires") { ProvinceId = 1, CountryId = 1 }
            };

            var repo = new Mock<IProvinceRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Province, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Province, object>>[]>()
            )).ReturnsAsync(provinces);

            // Mapear cada Province a ProvinceDto, usando una lista vacía de Visiteds
            mapper.Setup(m => m.Map<ProvinceDto>(It.IsAny<Province>()))
                .Returns((Province p) => new ProvinceDto(
                    p.ProvinceId,
                    p.Name,
                    p.CountryId,
                    new List<VisitedDto>() // Visiteds vacíos para el test
                ));

            var handler = new GetAllProvincesQueryHandler(repo.Object, mapper.Object);

            // Act
            var result = await handler.Handle(new GetAllProvincesQuery(), default);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Buenos Aires");
            repo.Verify(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Province, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Province, object>>[]>()
            ), Times.Once);
        }
    }
}
