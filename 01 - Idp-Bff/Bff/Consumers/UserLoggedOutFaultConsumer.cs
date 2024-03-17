using Charisma.AuthenticationManager.Extensions;
using Charisma.MessagingContracts.UsersManagement.User;
using MassTransit;
using System.Diagnostics;
namespace Charisma.AuthenticationManager.Consumer;

internal sealed class UserLoggedOutFaultConsumer : IConsumer<Fault<IUserLoggedOut>>
{
    private readonly ILogger<UserLoggedOutFaultConsumer> _logger;

    public UserLoggedOutFaultConsumer(ILogger<UserLoggedOutFaultConsumer> logger)
    {
        _logger = logger;
    }

    Task IConsumer<Fault<IUserLoggedOut>>.Consume(ConsumeContext<Fault<IUserLoggedOut>> context)
    {
        var startTime = Stopwatch.GetTimestamp();

        var exceptions = CharismaJson.Serialize(context.Message.Exceptions);

        var messageId = context.MessageId ?? Guid.Empty;
        _logger.LogLogoutFaultConsumer(messageId, exceptions, context.Message.Message.Id, context.Message.Message.Ip,
            context.Message.Message.UserAgent, Stopwatch.GetElapsedTime(startTime));

        return Task.CompletedTask;
    }
}
