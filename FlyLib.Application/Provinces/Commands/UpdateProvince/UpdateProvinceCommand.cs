using MediatR;

namespace FlyLib.Application.Provinces.Commands.UpdateProvince
{
    public class UpdateProvinceCommand : IRequest<Unit>
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public UpdateProvinceCommand() { }

        public UpdateProvinceCommand(int provinceId, string name, int countryId)
        {
            ProvinceId = provinceId;
            Name = name;
            CountryId = countryId;
        }
    }
}
