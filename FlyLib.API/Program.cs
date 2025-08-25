using CorrelationId;
using CorrelationId.DependencyInjection;
using FlyLib.API.Configurations;
using FlyLib.API.Extensions;
using FlyLib.API.Middleware;
using FlyLib.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// NUESTROS SERVICIOS
var isTest = builder.Environment.EnvironmentName == "Test";

if (!isTest)
{
    builder.Services.AddDbContext<FlyLibDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddFlyLibraryServices(builder.Configuration, useInMemory: isTest);
}


if (!isTest)
{
    builder.Services.AddDefaultCorrelationId();
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<FlyLibDbContext>("Database", tags: new[] { "ready" });
}

var app = builder.Build();

if (!isTest)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();
        db.Database.Migrate();

        // Crear roles si no existen
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}

// Middleware de errores (como IMiddleware)
app.UseMiddleware<GlobalExceptionMiddleware>();

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

if (!isTest)
{
    app.UseCorrelationId();
}

app.UseCors("FrontendCors");
app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (!isTest)
{
    // Health endpoints
    app.MapHealthChecks("/healthz"); // Liveness
    app.MapHealthChecks("/readyz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready")
    });
}

app.Run();

public partial class Program { }