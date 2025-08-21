using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Application.Provinces.Queries.GetProvinceByName;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Provinces
{
    public class GetProvinceByNameQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsProvinceByName()
        {
            var province = new Province("Buenos Aires") { ProvinceId = 1, CountryId = 1 };
            var repo = new Mock<IProvinceRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByNameAsync("Buenos Aires", default)).ReturnsAsync(province);
            mapper.Setup(m => m.Map<ProvinceDto>(province)).Returns(new ProvinceDto(1, "Buenos Aires", 1, new List<VisitedDto>()));

            var handler = new GetProvinceByNameQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetProvinceByNameQuery("Buenos Aires"), default);

            result.Should().NotBeNull();
            result.Name.Should().Be("Buenos Aires");
            repo.Verify(r => r.GetByNameAsync("Buenos Aires", default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenProvinceDoesNotExist()
        {
            var repo = new Mock<IProvinceRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByNameAsync("NoExiste", default)).ReturnsAsync((Province)null);

            var handler = new GetProvinceByNameQueryHandler(repo.Object, mapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetProvinceByNameQuery("NoExiste"), default));
        }
    }
}
