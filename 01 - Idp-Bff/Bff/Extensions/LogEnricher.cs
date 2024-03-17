using Charisma.AuthenticationManager.Middlewares;

namespace Charisma.AuthenticationManager.Extensions;

public static class LogEnricher
{
    public static IApplicationBuilder UseLogEnricher(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogEnricherMiddleware>();
        return app;
    }

    public static void UseExecptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
