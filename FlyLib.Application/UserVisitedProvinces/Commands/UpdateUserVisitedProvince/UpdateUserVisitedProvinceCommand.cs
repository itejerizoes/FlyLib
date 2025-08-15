using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Commands.UpdateUserVisitedProvince
{
    public sealed record UpdateUserVisitedProvinceCommand(int Id, string UserId, int ProvinceId, ICollection<VisitPhoto> VisitPhotos) : IRequest<Unit>;
}
