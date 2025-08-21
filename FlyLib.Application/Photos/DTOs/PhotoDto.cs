namespace FlyLib.Application.Photos.DTOs
{
    public sealed class PhotoDto
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public string? Description { get; set; }
        public int VisitedId { get; set; }

        public PhotoDto() { }

        public PhotoDto(int photoId, string url, string? description, int visitedId)
        {
            PhotoId = photoId;
            Url = url;
            Description = description;
            VisitedId = visitedId;
        }
    }
}
