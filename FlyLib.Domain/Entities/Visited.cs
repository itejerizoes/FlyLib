namespace FlyLib.Domain.Entities
{
    public class Visited
    {
        public int VisitId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
        public string? Description { get; set; }
        public DateTime VisitedAt { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();

        public Visited(int provinceId)
        {
            if (provinceId == 0)
                throw new ArgumentException("Tiene que estar vinculado a una provincia", nameof(provinceId));
            ProvinceId = provinceId;
        }
    }
}
