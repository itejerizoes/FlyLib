using FlyLib.API.Extensions;
using FlyLib.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NUESTROS SERVICIOS
builder.Services.AddFlyLibraryServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Middleware de errores (como IMiddleware)
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();
app.Run();
