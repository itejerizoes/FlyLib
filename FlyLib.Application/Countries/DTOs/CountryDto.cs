using FlyLib.Application.Provinces.DTOs;

namespace FlyLib.Application.Countries.DTOs
{
    public sealed class CountryDto
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string? IsoCode { get; set; }
        public IEnumerable<ProvinceDto> Provinces { get; set; }

        public CountryDto() { }

        public CountryDto(int countryId, string name, string? isoCode, IEnumerable<ProvinceDto> provinces)
        {
            CountryId = countryId;
            Name = name;
            IsoCode = isoCode;
            Provinces = provinces;
        }
    }
}
