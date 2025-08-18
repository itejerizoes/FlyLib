namespace FlyLib.API.DTOs.v1.Countries.Requests
{
    public record UpdateCountryRequestV1(int CountryId, string Name, string IsoCode);
}
