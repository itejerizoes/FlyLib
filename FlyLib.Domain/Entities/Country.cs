namespace FlyLib.Domain.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IsoCode { get; set; } = string.Empty;
        public ICollection<Province> Provinces { get; set; } = new List<Province>();

        public Country(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del país no puede estar vacío.", nameof(name));
            Name = name;
        }
    }
}
