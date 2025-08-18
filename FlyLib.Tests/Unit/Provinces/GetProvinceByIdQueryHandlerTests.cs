using FluentAssertions;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Application.Provinces.Queries.GetProvinceById;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Provinces
{
    public class GetProvinceByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsProvinceById()
        {
            var province = new Province("Buenos Aires") { ProvinceId = 1, CountryId = 1 };
            var repo = new Mock<IProvinceRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(province);
            mapper.Setup(m => m.Map<ProvinceDto>(province)).Returns(new ProvinceDto(1, "Buenos Aires", 1));

            var handler = new GetProvinceByIdQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetProvinceByIdQuery(1), default);

            result.Should().NotBeNull();
            result.Name.Should().Be("Buenos Aires");
            repo.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        }
    }
}
