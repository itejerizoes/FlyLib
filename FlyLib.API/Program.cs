using FlyLib.API.Configurations;
using FlyLib.API.Extensions;
using FlyLib.API.Middleware;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

//Configuraciuon Swaggerbuilder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FlyLib API",
        Version = "v1",
        Description = "API para registrar viajes y fotos de FlyLib",
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
        c.IncludeXmlComments(xmlPath);
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// NUESTROS SERVICIOS
builder.Services.AddFlyLibraryServices(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"FlyLib API {description.GroupName.ToUpperInvariant()}");
        }
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Middleware de errores (como IMiddleware)
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();
app.Run();
