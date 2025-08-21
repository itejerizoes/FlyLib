using FlyLib.Application.Provinces.DTOs;

namespace FlyLib.Application.Countries.DTOs
{
    public sealed record CountryDto(int CountryId, string Name, string? IsoCode, IEnumerable<ProvinceDto> Provinces);
}
