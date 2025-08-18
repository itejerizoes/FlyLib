using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Users
{
    public class CreateUserHandlerTests
    {
        [Fact]
        public async Task Creates_user_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<IUserRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.AddAsync(It.IsAny<User>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new CreateUserCommandHandler(repo.Object, uow.Object, mapper);

            var result = await handler.Handle(new CreateUserCommand("usuario1", "usuario1"), default);

            result.Should().BeOfType<UserDto>();
            result.DisplayName.Should().Be("usuario1");
            repo.Verify(r => r.AddAsync(It.IsAny<User>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
