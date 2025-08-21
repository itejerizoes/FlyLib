namespace FlyLib.API.DTOs.v1.Photos.Responses
{
    public class PhotoResponseV1
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string? Description { get; set; }
        public int VisitedId { get; set; }

        public PhotoResponseV1() { }

        public PhotoResponseV1(int id, string url, string? description, int visitedId)
        {
            Id = id;
            Url = url;
            Description = description;
            VisitedId = visitedId;
        }
    }
}
