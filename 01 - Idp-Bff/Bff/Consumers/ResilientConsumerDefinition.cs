using MassTransit;

namespace Charisma.AuthenticationManager.Consumer;

internal class ResilientConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer> where TConsumer : class, IConsumer
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseInMemoryInboxOutbox(context);
        endpointConfigurator.DiscardFaultedMessages();
        consumerConfigurator.UseMessageRetry(m => m.Immediate(10));
    }
}
