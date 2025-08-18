using AutoMapper;
using FlyLib.API.DTOs.v2.Countries.Requests;
using FlyLib.API.DTOs.v2.Countries.Responses;
using FlyLib.Application.Countries.Commands.CreateCountry;
using FlyLib.Application.Countries.Commands.UpdateCountry;
using FlyLib.Domain.Entities;

namespace FlyLib.API.Mappings.v2
{
    public class MappingProfileV2 : Profile
    {
        public MappingProfileV2()
        {

            // Request → Commands
            CreateMap<CreateCountryRequestV2, CreateCountryCommand>();
            CreateMap<UpdateCountryRequestV2, UpdateCountryCommand>();

            // Domain → Response
            CreateMap<Country, CountryResponseV2>();
        }
    }
}
