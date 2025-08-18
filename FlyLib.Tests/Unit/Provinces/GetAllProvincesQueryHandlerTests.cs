using FluentAssertions;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Application.Provinces.Queries.GetAllProvinces;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Provinces
{
    public class GetAllProvincesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllProvinces()
        {
            var provinces = new List<Province> { new Province("Buenos Aires") { ProvinceId = 1 } };
            var repo = new Mock<IProvinceRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetAllAsync(default)).ReturnsAsync(provinces);
            mapper.Setup(m => m.Map<ProvinceDto>(It.IsAny<Province>()))
                .Returns((Province p) => new ProvinceDto(p.ProvinceId, p.Name, p.CountryId));

            var handler = new GetAllProvincesQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetAllProvincesQuery(), default);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Buenos Aires");
            repo.Verify(r => r.GetAllAsync(default), Times.Once);
        }
    }
}
