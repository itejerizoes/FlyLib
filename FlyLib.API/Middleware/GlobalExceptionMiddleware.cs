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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception at {Path}", context.Request.Path);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var payload = _env.IsDevelopment()
                    ? new { statusCode = context.Response.StatusCode, message = ex.Message, traceId = context.TraceIdentifier }
                    : new { statusCode = context.Response.StatusCode, message = "Ocurrió un error inesperado." };

                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}
