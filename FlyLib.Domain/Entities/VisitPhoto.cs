namespace FlyLib.Domain.Entities
{
    public class VisitPhoto
    {
        public int VisitPhotoId { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UserVisitedProvinceId { get; set; }
        public UserVisitedProvince? UserVisitedProvince { get; set; }
    }
}
