using MediatR;
using Microsoft.Extensions.Logging;

namespace FlyLib.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            _logger.LogInformation("Handling {RequestName} with payload {@Request}", typeof(TRequest).Name, request);
            var response = await next();
            _logger.LogInformation("Handled {RequestName} with response {@Response}", typeof(TRequest).Name, response);
            return response;
        }
    }
}
