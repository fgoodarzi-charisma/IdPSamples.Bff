using Charisma.AuthenticationManager.Extensions;
using MassTransit;
using System.Diagnostics;
using System.Text.Json;

namespace Charisma.AuthenticationManager.Consumer;

internal sealed class FaultConsumer : IConsumer<Fault>
{
    private readonly ILogger<FaultConsumer> _logger;

    public FaultConsumer(ILogger<FaultConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Fault> context)
    {
        var startTime = Stopwatch.GetTimestamp();

        var exceptions = CharismaJson.Serialize(context.Message.Exceptions);

        var messageId = context.MessageId ?? Guid.Empty;
        _logger.LogGeneralFaultConsumer(messageId, exceptions, Stopwatch.GetElapsedTime(startTime));

        return Task.CompletedTask;
    }
}