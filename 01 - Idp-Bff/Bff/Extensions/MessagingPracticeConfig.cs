namespace Charisma.AuthenticationManager.Extensions;

public sealed class MessagingPracticeConfig
{
    public string? RabbitMqConnectionString { get; set; }
    public bool StartBusAutomatically { get; set; } = true;
}
