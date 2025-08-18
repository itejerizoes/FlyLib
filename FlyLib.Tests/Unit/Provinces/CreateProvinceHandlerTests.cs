using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Provinces.Commands.CreateProvince;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Provinces
{
    public class CreateProvinceHandlerTests
    {
        [Fact]
        public async Task Creates_province_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<IProvinceRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.AddAsync(It.IsAny<Province>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new CreateProvinceCommandHandler(repo.Object, uow.Object, mapper);

            var result = await handler.Handle(new CreateProvinceCommand("Buenos Aires", 1), default);

            result.Should().BeOfType<ProvinceDto>();
            result.Name.Should().Be("Buenos Aires");
            repo.Verify(r => r.AddAsync(It.IsAny<Province>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
