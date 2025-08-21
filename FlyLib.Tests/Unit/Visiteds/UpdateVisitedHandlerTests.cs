using AutoMapper;
using FluentAssertions;
using FlyLib.Application.Mapping;
using FlyLib.Application.Photos.DTOs;
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
            // Configuración del mapper (si tu handler no usa AutoMapper para Update, puedes omitirlo)
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfileApplication>()).CreateMapper();

            // Mock de repositorio y UnitOfWork
            var repo = new Mock<IVisitedRepository>();
            var uow = new Mock<IUnitOfWork>();

            // Entidades de prueba
            var visitedId = 1;
            var provinceId = 2;
            var visited = new Visited(provinceId) { VisitedId = visitedId, UserId = "user1" };

            repo.Setup(r => r.GetByIdAsync(visitedId, default)).ReturnsAsync(visited);
            repo.Setup(r => r.UpdateAsync(It.IsAny<Visited>(), default)).Returns(Task.CompletedTask);
            uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

            // Lista de fotos de prueba con todos los argumentos requeridos
            var photoDtos = new List<PhotoDto>
            {
                new PhotoDto(PhotoId: 0, Url: "http://example.com/photo1.jpg", Description: "Foto 1", VisitedId: visitedId),
                new PhotoDto(PhotoId: 0, Url: "http://example.com/photo2.jpg", Description: "Foto 2", VisitedId: visitedId)
            };

            // Crear handler
            var handler = new UpdateVisitedCommandHandler(repo.Object, uow.Object);

            // Ejecutar comando
            var result = await handler.Handle(
                new UpdateVisitedCommand(visitedId, "user1", provinceId, photoDtos),
                default
            );

            // Asserts
            result.Should().BeOfType<MediatR.Unit>();
            repo.Verify(r => r.UpdateAsync(It.IsAny<Visited>(), default), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
