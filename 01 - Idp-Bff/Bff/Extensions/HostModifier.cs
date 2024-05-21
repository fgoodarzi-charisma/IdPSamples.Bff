using Charisma.AuthenticationManager.Middlewares;

namespace Charisma.AuthenticationManager.Extensions;

public static class HostModifier
{
    public static IApplicationBuilder UseHostModifier(this IApplicationBuilder app)
    {
        app.UseMiddleware<HostMiddleware>();
        return app;
    }
}
