﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[START] Handle request={typeof(TRequest).Name} - Response={typeof(TResponse).Name} - RequestData={request}");

        var timer = new Stopwatch();
        timer.Start();

        var response = await next(cancellationToken);

        timer.Stop();

        if (timer.Elapsed.Seconds > 3)
        {
            logger.LogWarning($"[PERFORMANCE] The request {typeof(TRequest).Name} took {timer.Elapsed.Seconds} seconds!");
        }

        logger.LogInformation($"[END] Handled {typeof(TRequest).Name} with {response}");

        return response;
    }
}