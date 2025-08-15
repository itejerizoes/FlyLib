namespace FlyLib.API.Middleware
{
    public record ErrorResponse(int StatusCode, string Message, string? TraceId = null);
}
