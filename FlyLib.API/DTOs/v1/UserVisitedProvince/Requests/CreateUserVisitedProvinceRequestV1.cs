using FlyLib.Domain.Entities;

namespace FlyLib.API.DTOs.v1.UserVisitedProvince.Requests
{
    public record CreateUserVisitedProvinceRequestV1(string UserId, int ProvinceId, ICollection<VisitPhoto> VisitPhotos);
}
