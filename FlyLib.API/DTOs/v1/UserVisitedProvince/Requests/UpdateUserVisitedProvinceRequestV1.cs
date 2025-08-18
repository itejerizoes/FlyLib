using FlyLib.Domain.Entities;

namespace FlyLib.API.DTOs.v1.UserVisitedProvince.Requests
{
    public record UpdateUserVisitedProvinceRequestV1(int id, string UserId, int ProvinceId, ICollection<Domain.Entities.VisitPhoto> VisitPhotos);
}
