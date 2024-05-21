using Charisma.AuthenticationManager.Extensions;
using Charisma.AuthenticationManager.Services;
using Charisma.MessagingContracts.UsersManagement.User;
using MassTransit;
using System.Diagnostics;

namespace Charisma.AuthenticationManager.Consumer;

internal sealed class UserForceLoggedOutConsumer : IConsumer<IUserForceLoggedOut>
{
    private readonly ILogger<UserForceLoggedOutConsumer> _logger;
    private readonly ILogoutContext _logoutContext;

    public UserForceLoggedOutConsumer(ILogger<UserForceLoggedOutConsumer> logger, ILogoutContext logoutContext)
    {
        _logger = logger;
        _logoutContext = logoutContext;
    }

    public async Task Consume(ConsumeContext<IUserForceLoggedOut> context)
    {
        try
        {
            var startTime = Stopwatch.GetTimestamp();

            UserAgentInfo userAgentInfo = new()
            {
                UserId = context.Message.Id,
            };

            _logoutContext.Add(userAgentInfo.SessionId);

            var messageId = context.MessageId ?? Guid.Empty;
            _logger.LogForceLogoutConsumer(messageId, context.Message.Id, context.Message.Ip,
                context.Message.UserAgent, Stopwatch.GetElapsedTime(startTime));

            await Task.CompletedTask;
        }
        catch
        {
            await PublishFault(context);
        }
    }

    private static async Task PublishFault(ConsumeContext<IUserForceLoggedOut> context)
    {
        var faultMessage = new UserForceLoggedOut
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

        await context.Publish<Fault<IUserForceLoggedOut>>(fault, context.CancellationToken);
    }
}