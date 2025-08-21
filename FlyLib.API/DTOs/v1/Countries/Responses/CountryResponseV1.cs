using FlyLib.API.DTOs.v1.Provinces.Responses;

namespace FlyLib.API.DTOs.v1.Countries.Responses
{
    public record CountryResponseV1(int CountryId, string Name, string IsoCode, List<ProvinceResponseV1> Provinces);
}
