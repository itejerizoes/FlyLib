using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Application.Visiteds.Queries.GetVisitedById;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Visiteds
{
    public class GetVisitedByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsVisitedById()
        {
            var province = new Province("Santa Fe") { ProvinceId = 2 };
            var visited = new Visited(2) { VisitedId = 1, UserId = "user1", VisitedAt = DateTime.UtcNow };
            var repo = new Mock<IVisitedRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(visited);
            mapper.Setup(m => m.Map<VisitedDto>(visited)).Returns(new VisitedDto(1, "user1", 2, new List<PhotoDto>()));

            var handler = new GetVisitedByIdQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetVisitedByIdQuery(1), default);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            repo.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenVisitedDoesNotExist()
        {
            var repo = new Mock<IVisitedRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Visited)null);

            var handler = new GetVisitedByIdQueryHandler(repo.Object, mapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetVisitedByIdQuery(99), default));
        }
    }
}
