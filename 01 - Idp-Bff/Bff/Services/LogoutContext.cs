using Microsoft.Extensions.Caching.Distributed;

namespace Charisma.AuthenticationManager.Services;

internal sealed class LogoutContext : ILogoutContext
{
    private readonly IDistributedCache _distributedCache;

    public LogoutContext(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    void ILogoutContext.Add(string? sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return;
        }

        _distributedCache.SetString(sessionId, "1", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(30),
        });
    }

    void ILogoutContext.Remove(string sessionId)
    {
        _distributedCache.Remove(sessionId);
    }

    bool ILogoutContext.Contain(string sessionId)
    {
        var value = _distributedCache.GetString(sessionId);
        return !string.IsNullOrWhiteSpace(value);
    }
}
