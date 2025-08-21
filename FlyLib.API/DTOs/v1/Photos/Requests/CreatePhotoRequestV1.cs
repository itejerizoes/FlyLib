namespace FlyLib.API.DTOs.v1.Photos.Requests
{
    public class CreatePhotoRequestV1
    {
        public string Url { get; set; }
        public string? Description { get; set; }
        public int VisitedId { get; set; }

        public CreatePhotoRequestV1() { }

        public CreatePhotoRequestV1(string url, string? description, int visitedId)
        {
            Url = url;
            Description = description;
            VisitedId = visitedId;
        }
    }
}
