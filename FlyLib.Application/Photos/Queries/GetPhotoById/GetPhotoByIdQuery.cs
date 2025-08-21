using FlyLib.Application.Photos.DTOs;
using MediatR;

namespace FlyLib.Application.Photos.Queries.GetPhotoById
{
    public class GetPhotoByIdQuery : IRequest<PhotoDto?>
    {
        public int Id { get; set; }

        public GetPhotoByIdQuery() { }

        public GetPhotoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
