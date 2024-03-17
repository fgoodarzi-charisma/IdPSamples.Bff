using Serilog.Context;

namespace Charisma.AuthenticationManager.Middlewares;

internal sealed class LogEnricherMiddleware
{
    private readonly RequestDelegate _next;

    public LogEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
        {
            return _next(context);
        }
    }
}
