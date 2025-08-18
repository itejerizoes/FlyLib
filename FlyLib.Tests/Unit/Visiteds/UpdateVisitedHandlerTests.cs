using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Visiteds.Commands.UpdateVisited;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Visiteds
{
    public class UpdateVisitedHandlerTests
    {
        [Fact]
        public async Task Updates_visited_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<IVisitedRepository>();
            var uow = new Mock<IUnitOfWork>();

            var province = new Province("Santa Fe") { ProvinceId = 2 };
            var visited = new Visited(2) { VisitedId = 1, UserId = "user1" };
            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(visited);
            repo.Setup(r => r.UpdateAsync(It.IsAny<Visited>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new UpdateVisitedCommandHandler(repo.Object, uow.Object);

            var result = await handler.Handle(new UpdateVisitedCommand(1, "user1", 2, new List<Photo>()), default);

            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.UpdateAsync(It.IsAny<Visited>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
