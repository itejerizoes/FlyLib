using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FlyLib.API.Configurations
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "FlyLib API",
                    Version = description.ApiVersion.ToString(),
                    Description = "API de gestión de viajes y fotos con soporte de versionado",
                    Contact = new OpenApiContact
                    {
                        Name = "Ignacio Tejerizo",
                        Email = "ignacio.tejerizo.es@gmail.com",
                        Url = new Uri("https://github.com/itejerizoes/FlyLib")
                    }
                });

                // Incluir comentarios de XML para ejemplos y docs
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            }
        }
    }
}
