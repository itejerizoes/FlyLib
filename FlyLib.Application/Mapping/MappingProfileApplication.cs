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
            CreateMap<Photo, PhotoDto>()
                .ConstructUsing(p => MapPhoto(p));

            CreateMap<Visited, VisitedDto>()
                .ConstructUsing(v => MapVisited(v));

            CreateMap<Province, ProvinceDto>()
                .ConstructUsing(p => MapProvince(p));

            CreateMap<Country, CountryDto>()
                .ConstructUsing(c => MapCountry(c));

            CreateMap<User, UserDto>()
                .ConstructUsing(u => MapUser(u));
        }

        // Métodos privados para mapear DTOs anidados
        private static PhotoDto MapPhoto(Photo p) =>
            new PhotoDto(p.PhotoId, p.Url, p.Description, p.VisitedId);

        private static VisitedDto MapVisited(Visited v) =>
            new VisitedDto(
                v.VisitedId,
                v.UserId,
                v.ProvinceId,
                v.Photos != null
                    ? v.Photos.Select(MapPhoto).ToList()
                    : new List<PhotoDto>()
            );

        private static ProvinceDto MapProvince(Province p) =>
            new ProvinceDto(
                p.ProvinceId,
                p.Name,
                p.CountryId,
                p.Visiteds != null
                    ? p.Visiteds.Select(MapVisited).ToList()
                    : new List<VisitedDto>()
            );

        private static CountryDto MapCountry(Country c) =>
            new CountryDto(
                c.CountryId,
                c.Name,
                c.IsoCode,
                c.Provinces != null
                    ? c.Provinces.Select(MapProvince).ToList()
                    : new List<ProvinceDto>()
            );

        private static UserDto MapUser(User u) =>
            new UserDto(
                u.Id,
                u.UserName,
                u.DisplayName,
                u.Visiteds != null
                    ? u.Visiteds.Select(MapVisited).ToList()
                    : new List<VisitedDto>(),
                u.RefreshTokens
            );
    }
}
