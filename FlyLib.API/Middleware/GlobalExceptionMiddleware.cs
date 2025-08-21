using FluentValidation;
using FlyLib.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FlyLib.API.Middleware
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
            => (_logger, _env) = (logger, env);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

                // Manejo de status codes específicos que no lanzan excepción
                if (!context.Response.HasStarted)
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    {
                        await WriteProblemDetailsAsync(context, HttpStatusCode.NotFound, "Recurso no encontrado.");
                    }
                    else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                    {
                        await WriteProblemDetailsAsync(context, HttpStatusCode.Forbidden, "Acceso prohibido.");
                    }
                }
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación en {Path} [{Method}]", context.Request.Path, context.Request.Method);

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/problem+json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    var errors = ex.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    var problemDetails = new ValidationProblemDetails(errors)
                    {
                        Status = context.Response.StatusCode,
                        Title = "Error de validación.",
                        Detail = "Uno o más errores de validación ocurrieron.",
                        Instance = context.Request.Path
                    };

                    await WriteResponseAsync(context, problemDetails);
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "No encontrado en {Path} [{Method}]", context.Request.Path, context.Request.Method);
                if (!context.Response.HasStarted)
                {
                    await WriteProblemDetailsAsync(context, HttpStatusCode.NotFound, "No encontrado.", ex.Message);
                }
            }
            catch (ForbiddenException ex)
            {
                _logger.LogWarning(ex, "Acceso prohibido en {Path} [{Method}]", context.Request.Path, context.Request.Method);
                if (!context.Response.HasStarted)
                {
                    await WriteProblemDetailsAsync(context, HttpStatusCode.Forbidden, "Acceso prohibido.", ex.Message);
                }
            }
            catch (Exception ex)
            {
                var path = context.Request.Path;
                var method = context.Request.Method;
                var headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

                _logger.LogError(ex, "Excepción en {Method} {Path} | Headers: {Headers}", method, path, headers);

                if (!context.Response.HasStarted)
                {
                    await WriteProblemDetailsAsync(
                        context,
                        HttpStatusCode.InternalServerError,
                        _env.IsDevelopment() ? ex.Message : "Ocurrió un error inesperado.",
                        _env.IsDevelopment() ? ex.StackTrace : null
                    );
                }
            }
        }

        private async Task WriteProblemDetailsAsync(HttpContext context, HttpStatusCode statusCode, string title, string? detail = null)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("No se puede escribir el ProblemDetails porque la respuesta ya comenzó.");
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            await WriteResponseAsync(context, problemDetails);
        }

        private async Task WriteResponseAsync(HttpContext context, object content)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(content, options);
            await context.Response.WriteAsync(json);
        }
    }
}
