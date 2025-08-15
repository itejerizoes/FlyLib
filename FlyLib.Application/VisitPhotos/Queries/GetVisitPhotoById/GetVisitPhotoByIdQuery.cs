using FlyLib.Application.VisitPhotos.DTOs;
using MediatR;

namespace FlyLib.Application.VisitPhotos.Queries.GetVisitPhotoById
{
    public sealed record GetVisitPhotoByIdQuery(int Id) : IRequest<VisitPhotoDto?>;
}
