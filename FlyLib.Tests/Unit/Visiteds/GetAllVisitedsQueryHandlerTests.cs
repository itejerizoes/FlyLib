using FluentAssertions;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Application.Visiteds.Queries.GetAllVisiteds;
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

namespace FlyLib.Tests.Unit.Visiteds
{
    public class GetAllVisitedsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllVisiteds()
        {
            var province = new Province("Santa Fe") { ProvinceId = 2 };
            var visiteds = new List<Visited> { new Visited(2) { VisitedId = 1 } };
            var repo = new Mock<IVisitedRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Visited, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Visited, object>>[]>()
            )).ReturnsAsync(visiteds);
            mapper.Setup(m => m.Map<VisitedDto>(It.IsAny<Visited>()))
                .Returns((Visited v) => new VisitedDto(v.VisitedId, v.UserId, v.ProvinceId, new List<PhotoDto>()));

            var handler = new GetAllVisitedsQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetAllVisitedsQuery(), default);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(1);
            repo.Verify(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Visited, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Visited, object>>[]>()
            ), Times.Once);
        }
    }
}
