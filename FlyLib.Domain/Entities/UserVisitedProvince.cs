namespace FlyLib.Domain.Entities
{
    public class UserVisitedProvince
    {
        public int UserVisitedProvinceId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
        public ICollection<VisitPhoto> VisitPhotos { get; set; } = new List<VisitPhoto>();
    }
}
