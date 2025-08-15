using FlyLib.Application.VisitPhotos.DTOs;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Queries.GetAllVisitPhotos
{
    public sealed record GetAllVisitPhotosQuery() : IRequest<IEnumerable<VisitPhotoDto>>;
}
