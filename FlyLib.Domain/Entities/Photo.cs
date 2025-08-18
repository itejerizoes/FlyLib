namespace FlyLib.Domain.Entities
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int VisitedId { get; set; }
        public Visited? Visited { get; set; }
    }
}
