using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Application.Common.Behaviors;

public record LoggingBehavior<TRequest, TResponse>(Exception? Handling, object RequestName) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : this(null, default)
    {
        _logger = logger;
    }

    public Exception? Handling { get; private set; } = Handling;
    public object RequestName { get; private set; } = RequestName;
}
