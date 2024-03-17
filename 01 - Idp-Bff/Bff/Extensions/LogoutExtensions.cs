using Charisma.AuthenticationManager.Middlewares;

namespace Charisma.AuthenticationManager.Extensions;

public static class LogoutExtensions
{
    public static IApplicationBuilder UseLogoutHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogoutMiddleware>();
        return app;
    }
}
