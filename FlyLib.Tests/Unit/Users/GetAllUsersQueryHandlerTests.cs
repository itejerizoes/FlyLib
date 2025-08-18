using FluentAssertions;
using FlyLib.Application.Users.DTOs;
using FlyLib.Application.Users.Queries.GetAllUsers;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Users
{
    public class GetAllUsersQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllUsers()
        {
            var users = new List<User> { new User("usuario1") { Id = "1", DisplayName = "usuario1" } };
            var repo = new Mock<IUserRepository>();
            var mapper = new Mock<AutoMapper.IMapper>();

            repo.Setup(r => r.GetAllAsync(default)).ReturnsAsync(users);
            mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns((User u) => new UserDto(u.Id, u.UserName, u.DisplayName));

            var handler = new GetAllUsersQueryHandler(repo.Object, mapper.Object);

            var result = await handler.Handle(new GetAllUsersQuery(), default);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().DisplayName.Should().Be("usuario1");
            repo.Verify(r => r.GetAllAsync(default), Times.Once);
        }
    }
}
