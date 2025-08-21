using FlyLib.API.DTOs.v1.Provinces.Responses;

namespace FlyLib.API.DTOs.v1.Countries.Responses
{
    public class CountryResponseV1
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public List<ProvinceResponseV1> Provinces { get; set; }

        public CountryResponseV1() { }

        public CountryResponseV1(int countryId, string name, string isoCode, List<ProvinceResponseV1> provinces)
        {
            CountryId = countryId;
            Name = name;
            IsoCode = isoCode;
            Provinces = provinces;
        }
    }
}
