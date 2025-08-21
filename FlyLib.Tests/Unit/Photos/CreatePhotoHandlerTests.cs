using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Photos.Commands.CreatePhoto;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlyLib.Tests.Unit.Photos
{
    public class CreatePhotoHandlerTests
    {
        [Fact]
        public async Task Creates_photo_ok()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();
            var repo = new Mock<IPhotoRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.AddAsync(It.IsAny<Photo>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            var handler = new CreatePhotoCommandHandler(repo.Object, uow.Object, mapper);

            var result = await handler.Handle(new CreatePhotoCommand("http://url.com/foto.jpg", "Descripción", 1), default);

            result.Should().BeOfType<PhotoDto>();
            result.Url.Should().Be("http://url.com/foto.jpg");
            repo.Verify(r => r.AddAsync(It.IsAny<Photo>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
