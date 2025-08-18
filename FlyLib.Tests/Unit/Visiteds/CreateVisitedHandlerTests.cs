using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Visiteds.Commands.CreateVisited;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Visiteds
{
    public class CreateVisitedHandlerTests
    {
        [Fact]
        public async Task Creates_visited_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<IVisitedRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.AddAsync(It.IsAny<Visited>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new CreateVisitedCommandHandler(repo.Object, uow.Object, mapper);

            var result = await handler.Handle(new CreateVisitedCommand("user1", 2, new List<Photo>()), default);

            result.Should().BeOfType<VisitedDto>();
            result.UserId.Should().Be("user1");
            repo.Verify(r => r.AddAsync(It.IsAny<Visited>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
