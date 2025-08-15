namespace FlyLib.Domain.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Iso2 { get; set; } = string.Empty;
        public ICollection<Province> Provinces { get; set; } = new List<Province>();
    }
}
