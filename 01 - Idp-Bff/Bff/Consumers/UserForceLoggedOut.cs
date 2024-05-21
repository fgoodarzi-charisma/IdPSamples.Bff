using Charisma.MessagingContracts.UsersManagement.User;

namespace Charisma.AuthenticationManager.Consumer;

internal sealed class UserForceLoggedOut : IUserForceLoggedOut
{
    public long Id { get; init; }
    public string Ip { get; init; } = default!;
    public string UserAgent { get; init; } = default!;
    public string SessionId { get; init; } = default!;
}
