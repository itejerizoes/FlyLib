using FlyLib.Application.Photos.DTOs;
using MediatR;

namespace FlyLib.Application.Photos.Queries.GetPhotoById
{
    public sealed record GetPhotoByIdQuery(int Id) : IRequest<PhotoDto?>;
}
