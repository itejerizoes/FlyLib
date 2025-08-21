using FlyLib.Application.Provinces.DTOs;
using MediatR;

namespace FlyLib.Application.Provinces.Commands.CreateProvince
{
    public class CreateProvinceCommand : IRequest<ProvinceDto>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public CreateProvinceCommand() { }

        public CreateProvinceCommand(string name, int countryId)
        {
            Name = name;
            CountryId = countryId;
        }
    }
}
