using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Queries.GetProvinceById
{
    public class GetProvinceByIdQuery : IRequest<ProvinceDto?>
    {
        public int ProvinceId { get; set; }

        public GetProvinceByIdQuery() { }

        public GetProvinceByIdQuery(int provinceId)
        {
            ProvinceId = provinceId;
        }
    }
}
