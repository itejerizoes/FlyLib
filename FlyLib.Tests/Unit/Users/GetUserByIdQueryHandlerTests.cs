using FluentAssertions;
using FlyLib.Application.Common.Exceptions;
using FlyLib.Application.Users.DTOs;
using FlyLib.Application.Users.Queries.GetUserById;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Users
{
    public class GetUserByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsUserById()
        {
            var user = new User("usuario1") { Id = "1" };
            var repo = new Mock<IUserRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync("1", default)).ReturnsAsync(user);
            mapper.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto("1", "usuario1", "usuario1", new List<VisitedDto>(), new List<RefreshToken>()));

            var handler = new GetUserByIdQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetUserByIdQuery("1"), default);

            result.Should().NotBeNull();
            result.DisplayName.Should().Be("usuario1");
            repo.Verify(r => r.GetByIdAsync("1", default), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExist()
        {
            var repo = new Mock<IUserRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetByIdAsync("noexiste", default)).ReturnsAsync((User)null);

            var handler = new GetUserByIdQueryHandler(repo.Object, mapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetUserByIdQuery("noexiste"), default));
        }
    }
}
