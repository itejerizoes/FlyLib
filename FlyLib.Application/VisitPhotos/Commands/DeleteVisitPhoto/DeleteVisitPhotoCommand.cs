using MediatR;

namespace FlyLib.Application.VisitPhotos.Commands.DeleteVisitPhoto
{
    public sealed record DeleteVisitPhotoCommand(int Id) : IRequest<Unit>;
}
