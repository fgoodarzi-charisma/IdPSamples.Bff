namespace Charisma.AuthenticationManager.Middlewares;

public sealed class SchemeMiddleware
{
    private readonly RequestDelegate _next;

    public SchemeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (string.Equals(context.Request.Headers["X-Forwarded-Proto"], "https") ||
            string.Equals(context.Request.Headers["X-Forwarded-Scheme"], "https"))
        {
            context.Request.Scheme = "https";
        }
        context.Request.Headers.TryAdd("X-Csrf", "1");

        await _next(context);
    }
}
