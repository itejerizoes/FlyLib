using FlyLib.Application.Photos.DTOs;
using MediatR;

namespace FlyLib.Application.Photos.Commands.CreatePhoto
{
    public sealed record CreatePhotoCommand(string Url, string? Description, int VisitedId) : IRequest<PhotoDto>;
}
