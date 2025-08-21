using FlyLib.Application.Countries.DTOs;
using MediatR;

namespace FlyLib.Application.Countries.Commands.CreateCountry
{
    public class CreateCountryCommand : IRequest<CountryDto>
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public CreateCountryCommand() { }

        public CreateCountryCommand(string name, string isoCode)
        {
            Name = name;
            IsoCode = isoCode;
        }
    }
}
