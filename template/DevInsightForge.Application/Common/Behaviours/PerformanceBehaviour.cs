using DevInsightForge.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DevInsightForge.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly TokenServices _tokenServices;

    public PerformanceBehaviour(ILogger<TRequest> logger, TokenServices tokenService)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _tokenServices = tokenService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 1000)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _tokenServices.GetLoggedInUserId()?.Value;

            _logger.LogInformation("DevInsightForge.API Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                requestName, elapsedMilliseconds, userId, request);
        }

        return response;
    }
}
