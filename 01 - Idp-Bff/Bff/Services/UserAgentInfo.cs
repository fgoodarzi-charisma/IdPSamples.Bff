using Microsoft.IdentityModel.Tokens;

namespace Charisma.AuthenticationManager.Services;

public sealed class UserAgentInfo
{
    public long UserId { get; init; } = default!;
    public string Ip { get; init; } = default!;
    public string UserAgent { get; init; } = default!;

    public string Encode()
    {
        return Base64UrlEncoder.Encode($"{UserId}__{Ip}__{UserAgent}");
    }
}
