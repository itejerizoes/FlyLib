using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetProvinceByName
{
    public class GetProvinceByNameQuery : IRequest<ProvinceDto?>
    {
        public string Name { get; set; }

        public GetProvinceByNameQuery() { }

        public GetProvinceByNameQuery(string name)
        {
            Name = name;
        }
    }
}
