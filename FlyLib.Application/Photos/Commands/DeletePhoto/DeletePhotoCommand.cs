using MediatR;

namespace FlyLib.Application.Photos.Commands.DeletePhoto
{
    public sealed record DeletePhotoCommand(int Id) : IRequest<Unit>;
}
