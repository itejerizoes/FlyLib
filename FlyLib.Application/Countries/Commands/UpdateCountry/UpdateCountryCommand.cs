using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommand : IRequest<Unit>
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public UpdateCountryCommand() { }

        public UpdateCountryCommand(int countryId, string name, string isoCode)
        {
            CountryId = countryId;
            Name = name;
            IsoCode = isoCode;
        }
    }
}
