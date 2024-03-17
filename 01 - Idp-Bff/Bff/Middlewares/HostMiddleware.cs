using Charisma.AuthenticationManager.Extensions;

namespace Charisma.AuthenticationManager.Middlewares;

public sealed class HostMiddleware
{
    private readonly RequestDelegate _next;

    public HostMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<HostMiddleware> logger)
    {
        if (context.Request.Headers.TryGetValue("X-Host", out var host))
        {
            context.Request.Headers.Remove("Host");
            context.Request.Headers.TryAdd("Host", host);

            logger.HostReformedForReceivingRequest();
        }

        await _next(context);
    }
}