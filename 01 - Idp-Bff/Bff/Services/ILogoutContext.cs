namespace Charisma.AuthenticationManager.Services;

public interface ILogoutContext
{
    void Add(string key, UserAgentInfo user);

    void Remove(string key);

    bool Contain(string key);

    void MarkCookieToDelete(string cookie);

    bool ContainMarkToDeleteCookie(string cookie);
}
