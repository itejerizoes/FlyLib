using MediatR;

namespace FlyLib.Application.VisitPhotos.Commands.UpdateVisitPhoto
{
    public sealed record UpdateVisitPhotoCommand(int Id, string Url, string? Description, int UserVisitedProvinceId) : IRequest<Unit>;
}
