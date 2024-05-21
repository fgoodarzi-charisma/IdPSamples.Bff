using Charisma.AuthenticationManager.Extensions;
using Charisma.AuthenticationManager.Services;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Charisma.AuthenticationManager.Middlewares;

public sealed class LogoutMiddleware
{
    private readonly RequestDelegate _next;

    public LogoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogoutContext logoutContext, ILogger<LogoutMiddleware> logger, IConfiguration configuration)
    {
        if (context.Request.Path.ToString().Contains("bff/login", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var successUserIdParse = long.TryParse(context.User!.Claims.FirstOrDefault(c => c.Type == "sub")?.Value, out var userId);
        var idToken = await context.GetTokenAsync("id_token");
        string? sessionId = null;
        if (!string.IsNullOrWhiteSpace(idToken))
        {
            var jwtToken = new JwtSecurityToken(idToken);
            sessionId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        }
        UserAgentInfo userAgentInfo = new()
        {
            UserId = successUserIdParse ? userId : long.MaxValue,
            Ip = GetIp(context, logger),
            UserAgent = context.Request.Headers.UserAgent!,
            SessionId = sessionId,
        };

        logger.LogReceivingUserAgentInfo(userAgentInfo.UserId, userAgentInfo.Ip, userAgentInfo.UserAgent, sessionId);

        if (!string.IsNullOrWhiteSpace(sessionId) && logoutContext.Contain(sessionId))
        {
            var publicEndpoints = configuration.GetSection("PublicReverseProxy")?.Get<IDictionary<string, string>>() ??
                new Dictionary<string, string>();
            var isPublic = publicEndpoints.Keys.Select(k => $"/{k}/")
                .Any(k => context.Request.Path.ToString().StartsWith(k, StringComparison.OrdinalIgnoreCase));
            if (!isPublic)
            {
                context.Response.StatusCode = 401;
                logger.UserLoggedOut(userId, userAgentInfo.Ip, userAgentInfo.UserAgent);
                return;
            }
        }

        await _next(context);
    }

    private static string GetIp(HttpContext httpContext, ILogger<LogoutMiddleware> logger)
    {
        try
        {
            IPAddress ip;
            if (httpContext.Request.Headers.TryGetValue("x-forwarded-for", out var xForwardedFor))
            {
                logger.LogXForwardedFor(xForwardedFor);
                var xForwardedHeaderStr = xForwardedFor.ToString().Split(',')[0];
                xForwardedHeaderStr = string.Equals("::1", xForwardedHeaderStr) ? "127.0.0.1" : xForwardedHeaderStr;
                var ipHeader = xForwardedHeaderStr.Contains(':') ?
                    xForwardedHeaderStr.Remove(xForwardedHeaderStr.IndexOf(':')) :
                    xForwardedHeaderStr;
                ip = IPAddress.Parse(ipHeader);
            }
            else
            {
                ip = httpContext!.Connection.RemoteIpAddress!;
            }

            var ipv4 = ip.MapToIPv4();
            var ipv4Str = ipv4.ToString();

            logger.LogExtractedIp(ipv4Str);

            return ipv4Str;
        }
        catch (Exception ex)
        {
            logger.LogFailedOnObtainingIp(ex);
            return "Unknown IP";
        }
    }
}
