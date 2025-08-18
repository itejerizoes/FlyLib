using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Provinces.Commands.UpdateProvince;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Provinces
{
    public class UpdateProvinceHandlerTests
    {
        [Fact]
        public async Task Updates_province_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<IProvinceRepository>();
            var uow = new Mock<IUnitOfWork>();

            var province = new Province("Buenos Aires") { ProvinceId = 1, CountryId = 1 };
            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(province);
            repo.Setup(r => r.UpdateAsync(It.IsAny<Province>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new UpdateProvinceCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new UpdateProvinceCommand(1, "Buenos Aires", 1), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.UpdateAsync(It.IsAny<Province>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
