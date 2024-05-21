using Charisma.AuthenticationManager.Middlewares;

namespace Charisma.AuthenticationManager.Extensions;

public static class SchemeModifier
{
    public static IApplicationBuilder UseSchemeModifier(this IApplicationBuilder app)
    {
        app.UseMiddleware<SchemeMiddleware>();
        return app;
    }
}
