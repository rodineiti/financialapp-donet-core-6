using MediatR;
using System.Diagnostics;

namespace FinancialAppMvc.Logging;

public class MonitoringBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<MonitoringBehavior<TRequest, TResponse>> _logger;

    public MonitoringBehavior(ILogger<MonitoringBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("MediatR - Handling {RequestName} with payload {@Request}", requestName, request);

        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        _logger.LogInformation("MediatR - {RequestName} handled in {ElapsedMilliseconds} ms", requestName, stopwatch.ElapsedMilliseconds);

        return response;
    }
}