using AutoMapper;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Users.DTOs;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Domain.Entities;

namespace FlyLib.Application.Mapping
{
    public class MappingProfileApplication : Profile
    {
        public MappingProfileApplication()
        {
            CreateMap<Country, CountryDto>();
            CreateMap<User, UserDto>();
            CreateMap<Visited, VisitedDto>();
            CreateMap<Photo, PhotoDto>();
        }
    }
}
