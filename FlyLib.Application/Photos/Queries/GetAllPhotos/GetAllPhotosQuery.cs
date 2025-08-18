using FlyLib.Application.Photos.DTOs;
using MediatR;

namespace FlyLib.Application.Photos.Queries.GetAllPhotos
{
    public sealed record GetAllPhotosQuery() : IRequest<IEnumerable<PhotoDto>>;
}
