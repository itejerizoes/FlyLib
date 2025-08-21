using FlyLib.Application.Visiteds.DTOs;

namespace FlyLib.Application.Provinces.DTOs
{
    public sealed record ProvinceDto(int ProvinceId, string Name, int CountryId, IEnumerable<VisitedDto> Visiteds);
}
