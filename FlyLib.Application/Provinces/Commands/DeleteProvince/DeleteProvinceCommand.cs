using MediatR;

namespace FlyLib.Application.Provinces.Commands.DeleteProvince
{
    public sealed record DeleteProvinceCommand(int ProvinceId) : IRequest<Unit>;
}
