using Charisma.AuthenticationManager.Extensions;
using Charisma.AuthenticationManager.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace Charisma.AuthenticationManager.Middlewares;

public sealed class LogoutMiddleware
{
    private readonly RequestDelegate _next;

    public LogoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IConfiguration configuration, IDistributedCache cache,
        ILogoutContext logoutContext, ILogger<LogoutMiddleware> logger)
    {
        var successUserIdParse = long.TryParse(context.User!.Claims.FirstOrDefault(c => c.Type == "sub")?.Value, out var userId);
        UserAgentInfo userAgentInfo = new()
        {
            UserId = successUserIdParse ? userId : long.MaxValue,
            Ip = GetIp(context, logger),
            UserAgent = context.Request.Headers.UserAgent!,
        };

        var userInfoHashKey = userAgentInfo.Encode();

        logger.LogReceivingUserAgentInfo(userAgentInfo.UserId, userAgentInfo.Ip, userAgentInfo.UserAgent, userInfoHashKey);

        if (logoutContext.Contain(userInfoHashKey) || cache.Get(userInfoHashKey) is not null)
        {
            context.Response.Cookies.Delete($"bff.session.{configuration.GetValue<string>("Sts:ApplicationRoute")}");
            context.Request.Headers.Clear();
            logoutContext.Remove(userInfoHashKey);
            cache.Set(userInfoHashKey, [1], new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(2),
            });

            logger.UserLoggedOut(userId, userAgentInfo.Ip, userAgentInfo.UserAgent);
        }

        await _next(context);
    }

    private static string GetIp(HttpContext httpContext, ILogger<LogoutMiddleware> logger)
    {
        IPAddress ip;
        if (httpContext.Request.Headers.TryGetValue("x-forwarded-for", out var xForwardedFor))
        {
            logger.LogXForwardedFor(xForwardedFor);
            var xForwardedHeaderStr = xForwardedFor.ToString().Split(',')[0];
            xForwardedHeaderStr = string.Equals("::1", xForwardedHeaderStr) ? "0.0.0.1" : xForwardedHeaderStr;
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
}
