using FlyLib.Application.Photos.DTOs;
using MediatR;

namespace FlyLib.Application.Photos.Commands.CreatePhoto
{
    public class CreatePhotoCommand : IRequest<PhotoDto>
    {
        public string Url { get; set; }
        public string? Description { get; set; }
        public int VisitedId { get; set; }

        public CreatePhotoCommand() { }

        public CreatePhotoCommand(string url, string? description, int visitedId)
        {
            Url = url;
            Description = description;
            VisitedId = visitedId;
        }
    }
}
