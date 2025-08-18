using AutoMapper;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Users.DTOs;
using FlyLib.Application.UserVisitedProvinces.DTOs;
using FlyLib.Application.VisitPhotos.DTOs;
using FlyLib.Domain.Entities;

namespace FlyLib.Application.Mapping
{
    public class MappingProfileApplication : Profile
    {
        public MappingProfileApplication()
        {
            CreateMap<Country, CountryDto>();
            CreateMap<User, UserDto>();
            CreateMap<UserVisitedProvince, UserVisitedProvinceDto>();
            CreateMap<VisitPhoto, VisitPhotoDto>();
        }
    }
}
