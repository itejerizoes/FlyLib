namespace FlyLib.API.DTOs.v2.Countries.Requests
{
    public record UpdateCountryRequestV2(int CountryId, string Name, string IsoCode, string Continent);
}
