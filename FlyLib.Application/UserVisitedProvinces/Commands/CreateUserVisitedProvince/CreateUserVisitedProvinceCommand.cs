using FlyLib.Application.UserVisitedProvinces.DTOs;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Commands.CreateUserVisitedProvince
{
    public sealed record CreateUserVisitedProvinceCommand(string UserId, int ProvinceId, ICollection<VisitPhoto> VisitPhotos) : IRequest<UserVisitedProvinceDto>;
}
