using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Commands.DeleteUserVisitedProvince
{
    public sealed record DeleteUserVisitedProvinceCommand(int Id) : IRequest<Unit>;
}
