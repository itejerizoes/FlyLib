using FlyLib.Application.Photos.DTOs;
using MediatR;

namespace FlyLib.Application.Photos.Queries.GetAllPhotos
{
    public class GetAllPhotosQuery : IRequest<IEnumerable<PhotoDto>>
    {
        public GetAllPhotosQuery() { }
    }
}
