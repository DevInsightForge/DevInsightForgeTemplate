using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DevInsightForge.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 1000)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogInformation("DevInsightForge.API Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                requestName, elapsedMilliseconds, request);
        }

        return response;
    }
}
