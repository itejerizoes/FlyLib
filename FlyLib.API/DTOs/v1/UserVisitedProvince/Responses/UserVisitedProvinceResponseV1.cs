using FlyLib.Domain.Entities;

namespace FlyLib.API.DTOs.v1.UserVisitedProvince.Responses
{
    public record UserVisitedProvinceResponseV1(int id, string UserId, int ProvinceId, ICollection<Domain.Entities.VisitPhoto> VisitPhotos);
}
