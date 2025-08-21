using MediatR;

namespace FlyLib.Application.Photos.Commands.UpdatePhoto
{
    public class UpdatePhotoCommand : IRequest<Unit>
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public string? Description { get; set; }
        public int VisitedId { get; set; }

        public UpdatePhotoCommand() { }

        public UpdatePhotoCommand(int photoId, string url, string? description, int visitedId)
        {
            PhotoId = photoId;
            Url = url;
            Description = description;
            VisitedId = visitedId;
        }
    }
}
