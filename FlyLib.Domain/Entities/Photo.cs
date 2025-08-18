namespace FlyLib.Domain.Entities
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int VisitedId { get; set; }
        public Visited? Visited { get; set; }

        public Photo(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("La URL de la foto no puede estar vacía.", nameof(url));
            Url = url;
        }
    }
}
