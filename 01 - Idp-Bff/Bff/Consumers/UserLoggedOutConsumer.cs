using Charisma.AuthenticationManager.Extensions;
using Charisma.AuthenticationManager.Services;
using Charisma.MessagingContracts.UsersManagement.User;
using MassTransit;
using System.Diagnostics;

namespace Charisma.AuthenticationManager.Consumer;

internal sealed class UserLoggedOutConsumer : IConsumer<IUserLoggedOut>
{
    private readonly ILogger<UserLoggedOutConsumer> _logger;
    private readonly ILogoutContext _logoutContext;

    public UserLoggedOutConsumer(ILogger<UserLoggedOutConsumer> logger, ILogoutContext logoutContext)
    {
        _logger = logger;
        _logoutContext = logoutContext;
    }

    public async Task Consume(ConsumeContext<IUserLoggedOut> context)
    {
        try
        {
            var startTime = Stopwatch.GetTimestamp();

            _logoutContext.Add(context.Message.SessionId);

            var messageId = context.MessageId ?? Guid.Empty;
            _logger.LogLogoutConsumer(messageId, context.Message.Id, context.Message.Ip,
                context.Message.UserAgent, context.Message.SessionId, Stopwatch.GetElapsedTime(startTime));

            await Task.CompletedTask;
        }
        catch
        {
            await PublishFault(context);
        }
    }

    private static async Task PublishFault(ConsumeContext<IUserLoggedOut> context)
    {
        var faultMessage = new UserLoggedOut
        {
            Id = context.Message.Id,
            Ip = context.Message.Ip,
            UserAgent = context.Message.UserAgent,
        };

        var fault = new
        {
            Message = faultMessage,
            FaultId = Guid.NewGuid(),
            FaultedMessageId = Guid.NewGuid(),
            DateTime = DateTime.UtcNow,
            context.Host,
        };

        await context.Publish<Fault<IUserLoggedOut>>(fault, context.CancellationToken);
    }
}
