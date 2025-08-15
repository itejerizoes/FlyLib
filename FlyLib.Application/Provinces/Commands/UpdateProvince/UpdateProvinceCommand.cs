using MediatR;

namespace FlyLib.Application.Provinces.Commands.UpdateProvince
{
    public sealed record UpdateProvinceCommand(int ProvinceId, string Name, int CountryId) : IRequest<Unit>;
}
