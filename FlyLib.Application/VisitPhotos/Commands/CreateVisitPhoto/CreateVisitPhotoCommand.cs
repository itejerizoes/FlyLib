using FlyLib.Application.VisitPhotos.DTOs;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Commands.CreateVisitPhoto
{
    public sealed record CreateVisitPhotoCommand(string PhotoUrl, string? Description, int UserVisitedProvinceId) : IRequest<VisitPhotoDto>;
}
