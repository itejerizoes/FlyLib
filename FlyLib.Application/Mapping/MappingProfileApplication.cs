using AutoMapper;
using FlyLib.Application.Countries.DTOs;
using FlyLib.Application.Users.DTOs;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Application.Photos.DTOs;
using FlyLib.Application.Provinces.DTOs;
using FlyLib.Domain.Entities;

namespace FlyLib.Application.Mapping
{
    public class MappingProfileApplication : Profile
    {
        public MappingProfileApplication()
        {
            CreateMap<Country, CountryDto>()
                .ConstructUsing(src =>
                    new CountryDto(src.CountryId, src.Name, src.Iso2));
            CreateMap<Province, ProvinceDto>()
                .ConstructUsing(src =>
                    new ProvinceDto(src.ProvinceId, src.Name, src.CountryId));
            CreateMap<User, UserDto>()
                .ConstructUsing(src =>
                    new UserDto(src.Id, src.Email, src.DisplayName));
            CreateMap<Visited, VisitedDto>()
                .ConstructUsing(src =>
                    new VisitedDto(src.VisitedId, src.UserId, src.ProvinceId, src.Photos));
            CreateMap<Photo, PhotoDto>()
                .ConstructUsing(src =>
                    new PhotoDto(src.PhotoId, src.Url, src.Description, src.VisitedId));
        }
    }
}
