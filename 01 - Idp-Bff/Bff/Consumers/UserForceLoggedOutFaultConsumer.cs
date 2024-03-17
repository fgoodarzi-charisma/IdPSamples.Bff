using Charisma.AuthenticationManager.Extensions;
using Charisma.MessagingContracts.UsersManagement.User;
using MassTransit;
using System.Diagnostics;
using System.Text.Json;
namespace Charisma.AuthenticationManager.Consumer;

internal sealed class UserForceLoggedOutFaultConsumer : IConsumer<Fault<IUserForceLoggedOut>>
{
    private readonly ILogger<UserForceLoggedOutFaultConsumer> _logger;

    public UserForceLoggedOutFaultConsumer(ILogger<UserForceLoggedOutFaultConsumer> logger)
    {
        _logger = logger;
    }

    Task IConsumer<Fault<IUserForceLoggedOut>>.Consume(ConsumeContext<Fault<IUserForceLoggedOut>> context)
    {
        var startTime = Stopwatch.GetTimestamp();

        var exceptions = CharismaJson.Serialize(context.Message.Exceptions);

        var messageId = context.MessageId ?? Guid.Empty;
        _logger.LogForceLogoutFaultConsumer(messageId, exceptions, context.Message.Message.Id, context.Message.Message.Ip,
            context.Message.Message.UserAgent, Stopwatch.GetElapsedTime(startTime));

        return Task.CompletedTask;
    }
}
