using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using FlyLib.API.Mappings.v1;
using FlyLib.API.Mappings.v2;
using FlyLib.API.Middleware;
using FlyLib.Application.Common.Behaviors;
using FlyLib.Application.Mapping;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Identity.Jwt;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Infrastructure.Repositories;
using FlyLib.Infrastructure.Services;
using FlyLib.Infrastructure.Storages;
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
        public static IServiceCollection AddFlyLibraryServices(this IServiceCollection services, IConfiguration config)
        {
            // DbContext
            services.AddDbContext<FlyLibDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

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
            services.AddMediatR(typeof(Program).Assembly);
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

            // Auth (si corresponde): AddAuthentication().AddJwtBearer(...) + Roles
            // Configuración de Identity
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
                        ValidateAudience = false,
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
                })
                .AddMicrosoftAccount("Microsoft", options =>
                {
                    options.ClientId = config["Authentication:Microsoft:ClientId"]!;
                    options.ClientSecret = config["Authentication:Microsoft:ClientSecret"]!;
                });

            //Health Checks
            services.AddHealthChecks().AddDbContextCheck<FlyLibDbContext>("Database", tags: new[] { "ready" });

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendCors", policy =>
                {
                    policy.WithOrigins("https://flylib-frontend.com") // Cambia por la URL real de tu frontend
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

            //Azure Storage Blobs
            services.AddSingleton(new BlobServiceClient(config.GetConnectionString("AzureBlobStorage")));
            services.AddScoped<BlobStorageService>();

            return services;
        }
    }
}
