namespace Charisma.AuthenticationManager.Services;

public interface ILogoutContext
{
    void Add(string? sessionId);

    void Remove(string sessionId);

    bool Contain(string sessionId);
}
