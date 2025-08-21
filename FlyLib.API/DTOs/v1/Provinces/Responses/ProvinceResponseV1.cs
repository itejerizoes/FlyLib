using FlyLib.API.DTOs.v1.Visited.Responses;

namespace FlyLib.API.DTOs.v1.Provinces.Responses
{
    public class ProvinceResponseV1
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public List<VisitedResponseV1> Visiteds { get; set; }

        public ProvinceResponseV1() { }

        public ProvinceResponseV1(int provinceId, string name, int countryId, List<VisitedResponseV1> visiteds)
        {
            ProvinceId = provinceId;
            Name = name;
            CountryId = countryId;
            Visiteds = visiteds;
        }
    }
}
