using FlyLib.Application.Visiteds.DTOs;

namespace FlyLib.Application.Provinces.DTOs
{
    public sealed class ProvinceDto
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public IEnumerable<VisitedDto> Visiteds { get; set; }

        public ProvinceDto() { }

        public ProvinceDto(int provinceId, string name, int countryId, IEnumerable<VisitedDto> visiteds)
        {
            ProvinceId = provinceId;
            Name = name;
            CountryId = countryId;
            Visiteds = visiteds;
        }
    }
}
