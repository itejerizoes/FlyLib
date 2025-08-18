namespace FlyLib.Domain.Entities
{
    public class Province
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public ICollection<Visited> Visiteds { get; set; } = new List<Visited>();
    }
}
