using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetAllProvinces
{
    public class GetAllProvincesQuery : IRequest<IEnumerable<ProvinceDto>>
    {
        public GetAllProvincesQuery() { }
    }
}
