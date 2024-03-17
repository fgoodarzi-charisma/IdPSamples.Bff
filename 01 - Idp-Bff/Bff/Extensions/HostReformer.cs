using Charisma.AuthenticationManager.Middlewares;

namespace Charisma.AuthenticationManager.Extensions;

public static class HostReformer
{
    public static IApplicationBuilder UseHostReformer(this IApplicationBuilder app)
    {
        app.UseMiddleware<HostMiddleware>();
        return app;
    }
}
