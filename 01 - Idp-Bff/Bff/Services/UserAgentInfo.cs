namespace Charisma.AuthenticationManager.Services;

public sealed class UserAgentInfo
{
    public long UserId { get; init; }
    public string Ip { get; init; } = default!;
    public string UserAgent { get; init; } = default!;
    public string? SessionId { get; init; }
}
