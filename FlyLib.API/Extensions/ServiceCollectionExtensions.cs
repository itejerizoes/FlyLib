using FluentValidation;
using FluentValidation.AspNetCore;
using FlyLib.API.Mappings.v1;
using FlyLib.API.Mappings.v2;
using FlyLib.API.Middleware;
using FlyLib.Application.Common.Behaviors;
using FlyLib.Application.Countries.Commands.CreateCountry;
using FlyLib.Application.Countries.Commands.DeleteCountry;
using FlyLib.Application.Countries.Commands.UpdateCountry;
using FlyLib.Application.Countries.Queries.GetAllCountries;
using FlyLib.Application.Countries.Queries.GetCountryById;
using FlyLib.Application.Countries.Queries.GetCountryByName;
using FlyLib.Application.Mapping;
using FlyLib.Application.Photos.Commands.CreatePhoto;
using FlyLib.Application.Photos.Commands.DeletePhoto;
using FlyLib.Application.Photos.Commands.UpdatePhoto;
using FlyLib.Application.Photos.Queries.GetAllPhotos;
using FlyLib.Application.Photos.Queries.GetPhotoById;
using FlyLib.Application.Provinces.Commands.CreateProvince;
using FlyLib.Application.Provinces.Commands.DeleteProvince;
using FlyLib.Application.Provinces.Commands.UpdateProvince;
using FlyLib.Application.Provinces.Queries.GetAllProvinces;
using FlyLib.Application.Provinces.Queries.GetProvinceById;
using FlyLib.Application.Provinces.Queries.GetProvinceByName;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.Commands.DeleteUser;
using FlyLib.Application.Users.Commands.UpdateUser;
using FlyLib.Application.Users.Queries.GetAllUsers;
using FlyLib.Application.Users.Queries.GetUserById;
using FlyLib.Application.Visiteds.Commands.CreateVisited;
using FlyLib.Application.Visiteds.Commands.DeleteVisited;
using FlyLib.Application.Visiteds.Commands.UpdateVisited;
using FlyLib.Application.Visiteds.Queries.GetAllVisiteds;
using FlyLib.Application.Visiteds.Queries.GetVisitedById;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Identity.Jwt;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Infrastructure.Repositories;
using FlyLib.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace FlyLib.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFlyLibraryServices(this IServiceCollection services, IConfiguration config, bool useInMemory = false)
        {
            // UoW + Repos
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVisitedRepository, VisitedRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            // MediatR + Behaviors
            services.AddScoped<RefreshTokenService>();
            services.AddMediatR(typeof(Program).Assembly);
            services.AddMediatR(typeof(CreateCountryCommand).Assembly);
            services.AddMediatR(typeof(CreateCountryCommandHandler).Assembly);
            services.AddMediatR(typeof(DeleteCountryCommand).Assembly);
            services.AddMediatR(typeof(DeleteCountryCommandHandler).Assembly);
            services.AddMediatR(typeof(UpdateCountryCommand).Assembly);
            services.AddMediatR(typeof(UpdateCountryCommandHandler).Assembly);
            services.AddMediatR(typeof(GetAllCountriesQuery).Assembly);
            services.AddMediatR(typeof(GetAllCountriesQueryHandler).Assembly);
            services.AddMediatR(typeof(GetCountryByIdQuery).Assembly);
            services.AddMediatR(typeof(GetCountryByIdQueryHandler).Assembly);
            services.AddMediatR(typeof(GetCountryByNameQuery).Assembly);
            services.AddMediatR(typeof(GetCountryByNameQueryHandler).Assembly);
            services.AddMediatR(typeof(CreateProvinceCommand).Assembly);
            services.AddMediatR(typeof(CreateProvinceCommandHandler).Assembly);
            services.AddMediatR(typeof(DeleteProvinceCommand).Assembly);
            services.AddMediatR(typeof(DeleteProvinceCommandHandler).Assembly);
            services.AddMediatR(typeof(UpdateProvinceCommand).Assembly);
            services.AddMediatR(typeof(UpdateProvinceCommandHandler).Assembly);
            services.AddMediatR(typeof(GetAllProvincesQuery).Assembly);
            services.AddMediatR(typeof(GetAllProvincesQueryHandler).Assembly);
            services.AddMediatR(typeof(GetProvinceByIdQuery).Assembly);
            services.AddMediatR(typeof(GetProvinceByIdQueryHandler).Assembly);
            services.AddMediatR(typeof(GetProvinceByNameQuery).Assembly);
            services.AddMediatR(typeof(GetProvinceByNameQueryHandler).Assembly);
            services.AddMediatR(typeof(CreateUserCommand).Assembly);
            services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);
            services.AddMediatR(typeof(DeleteUserCommand).Assembly);
            services.AddMediatR(typeof(DeleteUserCommandHandler).Assembly);
            services.AddMediatR(typeof(UpdateUserCommand).Assembly);
            services.AddMediatR(typeof(UpdateUserCommandHandler).Assembly);
            services.AddMediatR(typeof(GetAllUsersQuery).Assembly);
            services.AddMediatR(typeof(GetAllUsersQueryHandler).Assembly);
            services.AddMediatR(typeof(GetUserByIdQuery).Assembly);
            services.AddMediatR(typeof(GetUserByIdQueryHandler).Assembly);
            services.AddMediatR(typeof(CreateVisitedCommand).Assembly);
            services.AddMediatR(typeof(CreateVisitedCommandHandler).Assembly);
            services.AddMediatR(typeof(DeleteVisitedCommand).Assembly);
            services.AddMediatR(typeof(DeleteVisitedCommandHandler).Assembly);
            services.AddMediatR(typeof(UpdateVisitedCommand).Assembly);
            services.AddMediatR(typeof(UpdateVisitedCommandHandler).Assembly);
            services.AddMediatR(typeof(GetAllVisitedsQuery).Assembly);
            services.AddMediatR(typeof(GetAllVisitedsQueryHandler).Assembly);
            services.AddMediatR(typeof(GetVisitedByIdQuery).Assembly);
            services.AddMediatR(typeof(GetVisitedByIdQueryHandler).Assembly);
            services.AddMediatR(typeof(CreatePhotoCommand).Assembly);
            services.AddMediatR(typeof(CreatePhotoCommandHandler).Assembly);
            services.AddMediatR(typeof(DeletePhotoCommand).Assembly);
            services.AddMediatR(typeof(DeletePhotoCommandHandler).Assembly);
            services.AddMediatR(typeof(UpdatePhotoCommand).Assembly);
            services.AddMediatR(typeof(UpdatePhotoCommandHandler).Assembly);
            services.AddMediatR(typeof(GetAllPhotosQuery).Assembly);
            services.AddMediatR(typeof(GetAllPhotosQueryHandler).Assembly);
            services.AddMediatR(typeof(GetPhotoByIdQuery).Assembly);
            services.AddMediatR(typeof(GetPhotoByIdQueryHandler).Assembly);

            // Logging Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfileApplication>());
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfileV1>());
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfileV2>());

            //Versionado de la API
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV"; // ej: v1, v2
                opt.SubstituteApiVersionInUrl = true;
            });

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(MappingProfileV1).Assembly);

            // Middleware
            services.AddTransient<GlobalExceptionMiddleware>();

            if (!useInMemory)
            {
                services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<FlyLibDbContext>()
                    .AddDefaultTokenProviders();

                var jwtKey = config["Jwt:Key"] ?? "thoifel_marlo_123+";
                var jwtIssuer = config["Jwt:Issuer"] ?? "TestFlyLibrary";
                var jwtAudience = config["Jwt:Audience"] ?? "TestFlyClient";

                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtIssuer,
                            ValidAudience = jwtAudience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                        };
                    })
                    .AddGoogle("Google", options =>
                    {
                        options.ClientId = config["Authentication:Google:ClientId"]!;
                        options.ClientSecret = config["Authentication:Google:ClientSecret"]!;
                        options.SignInScheme = IdentityConstants.ExternalScheme;
                    });
            }

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendCors", policy =>
                {
                    policy.WithOrigins("http://localhost:3000") // Cambia por la URL real de tu frontend
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            //Rate Limiting
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100, // Máximo de peticiones
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));
                options.RejectionStatusCode = 429;
            });

            return services;
        }
    }
}
