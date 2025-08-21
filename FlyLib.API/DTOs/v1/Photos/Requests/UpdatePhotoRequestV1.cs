namespace FlyLib.API.DTOs.v1.Photos.Requests
{
    public class UpdatePhotoRequestV1
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public string? Description { get; set; }
        public int VisitedId { get; set; }

        public UpdatePhotoRequestV1() { }

        public UpdatePhotoRequestV1(int photoId, string url, string? description, int visitedId)
        {
            PhotoId = photoId;
            Url = url;
            Description = description;
            VisitedId = visitedId;
        }
    }
}
