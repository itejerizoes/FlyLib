using AutoMapper;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Countries.Responses;
using FlyLib.API.DTOs.v1.Provinces.Requests;
using FlyLib.API.DTOs.v1.Provinces.Responses;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
using FlyLib.API.DTOs.v1.Visited.Requests;
using FlyLib.API.DTOs.v1.Visited.Responses;
using FlyLib.Application.Countries.Commands.CreateCountry;
using FlyLib.Application.Countries.Commands.UpdateCountry;
using FlyLib.Application.Provinces.Commands.CreateProvince;
using FlyLib.Application.Provinces.Commands.UpdateProvince;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.Commands.UpdateUser;
using FlyLib.Application.Visiteds.Commands.CreateVisited;
using FlyLib.Application.Visiteds.Commands.UpdateVisited;
using FlyLib.Application.Photos.Commands.CreatePhoto;
using FlyLib.Application.Photos.Commands.UpdatePhoto;
using FlyLib.Domain.Entities;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Photos.Responses;

namespace FlyLib.API.Mappings.v1
{
    public class MappingProfileV1 : Profile
    {
        public MappingProfileV1()
        {

            // Request → Commands
            CreateMap<CreateCountryRequestV1, CreateCountryCommand>();
            CreateMap<UpdateCountryRequestV1, UpdateCountryCommand>();
            CreateMap<CreateProvinceRequestV1, CreateProvinceCommand>();
            CreateMap<UpdateProvinceRequestV1, UpdateProvinceCommand>();
            CreateMap<CreateUserRequestV1, CreateUserCommand>();
            CreateMap<UpdateUserRequestV1, UpdateUserCommand>();
            CreateMap<CreateVisitedRequestV1, CreateVisitedCommand>();
            CreateMap<UpdateVisitedRequestV1, UpdateVisitedCommand>();
            CreateMap<CreatePhotoRequestV1, CreatePhotoCommand>();
            CreateMap<UpdatePhotoRequestV1, UpdatePhotoCommand>();

            // Domain → Response
            CreateMap<Country, CountryResponseV1>();
            CreateMap<Province, ProvinceResponseV1>();
            CreateMap<User, UserResponseV1>();
            CreateMap<Visited, VisitedResponseV1>();
            CreateMap<Photo, PhotoResponseV1>();
        }
    }
}
