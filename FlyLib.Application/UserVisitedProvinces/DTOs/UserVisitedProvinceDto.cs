using FlyLib.Domain.Entities;

namespace FlyLib.Application.UserVisitedProvinces.DTOs
{
    public sealed record UserVisitedProvinceDto(int Id, string UserId, int ProvinceId, ICollection<VisitPhoto> VisitPhotos);
}
