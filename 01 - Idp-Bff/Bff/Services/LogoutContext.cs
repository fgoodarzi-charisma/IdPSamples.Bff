using Microsoft.Extensions.Caching.Distributed;

namespace Charisma.AuthenticationManager.Services;

internal sealed class LogoutContext : ILogoutContext
{
    private readonly IDistributedCache _distributedCache;

    public LogoutContext(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    void ILogoutContext.Add(string key, UserAgentInfo user)
    {
        _distributedCache.SetString(key, user.Encode(), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(7),
        });
    }

    void ILogoutContext.Remove(string key)
    {
        _distributedCache.Remove(key);
    }

    bool ILogoutContext.Contain(string key)
    {
        var value = _distributedCache.GetString(key);
        return !string.IsNullOrWhiteSpace(value);
    }

    void ILogoutContext.MarkCookieToDelete(string cookie)
    {
        _distributedCache.SetString(cookie, "1", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(7),
        });
    }

    bool ILogoutContext.ContainMarkToDeleteCookie(string cookie)
    {
        var value = _distributedCache.GetString(cookie);
        return !string.IsNullOrWhiteSpace(value);
    }
}
