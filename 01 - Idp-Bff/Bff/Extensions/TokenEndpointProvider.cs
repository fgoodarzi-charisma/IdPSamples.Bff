using Microsoft.AspNetCore.Authentication;

namespace Charisma.AuthenticationManager.Extensions;

public static class TokenEndpointProvider
{
    public static IApplicationBuilder MapTokenEndpoint(this WebApplication app)
    {
        app.MapGet("/bff/token", async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                logger.LogUnauthorizedUser();
                context.Response.StatusCode = 401;
                return;
            }

            var tokenEndpoint = app.Configuration.GetValue<string>("TokenEndpoint");
            if (!string.Equals(tokenEndpoint, "enabled", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogForbiddenTokenEndpoint();
                context.Response.StatusCode = 403;
                return;
            }

            var accessToken = await context.GetTokenAsync("access_token");
            var idToken = await context.GetTokenAsync("id_token");
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            var response = CharismaJson.Serialize(new List<object>
        {
            new { TokenName = "access_token", Token = accessToken, },
            new { TokenName = "id_token", Token = idToken, },
        });

            await context.Response.WriteAsync(response);
        });

        return app;
    }
}
