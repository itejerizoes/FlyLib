using MediatR;

namespace FlyLib.Application.Photos.Commands.UpdatePhoto
{
    public sealed record UpdatePhotoCommand(int Id, string Url, string? Description, int VisitedId) : IRequest<Unit>;
}
