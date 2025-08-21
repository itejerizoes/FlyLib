using FlyLib.API.DTOs.v1.Visited.Responses;

namespace FlyLib.API.DTOs.v1.Provinces.Responses
{
    public record ProvinceResponseV1(int ProvinceId, string Name, int CountryId, ICollection<VisitedResponseV1> Visiteds);
}
