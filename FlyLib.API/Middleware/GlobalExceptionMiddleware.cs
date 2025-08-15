using FluentValidation;
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

                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await HandleExceptionAsync(context, HttpStatusCode.NotFound, "Recurso no encontrado.");
                }
                else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    await HandleExceptionAsync(context, HttpStatusCode.Forbidden, "Acceso denegado.");
                }
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación en {Path} [{Method}]", context.Request.Path, context.Request.Method);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var validationResponse = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "Error de validación.",
                    details = errors
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(validationResponse));
            }
            catch (Exception ex)
            {
                var path = context.Request.Path;
                var method = context.Request.Method;
                var headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

                _logger.LogError(ex, "Excepción en {Method} {Path} | Headers: {Headers}", method, path, headers);

                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError,
                    _env.IsDevelopment() ? ex.Message : "Ocurrió un error inesperado.",
                    _env.IsDevelopment() ? ex.StackTrace : null);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, string? traceId = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                TraceId = traceId
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
