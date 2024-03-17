using Charisma.AuthenticationManager.Consumer;
using Charisma.MessagingContracts.UsersManagement.User;
using MassTransit;
using Serilog;

namespace Charisma.AuthenticationManager.Extensions;

internal static class MessagingPracticeBootstrapper
{
    public static IServiceCollection AddMessagingPractice(this IServiceCollection services, Action<MessagingPracticeConfig> config)
    {
        var newConfig = new MessagingPracticeConfig();
        config.Invoke(newConfig);

        if (string.IsNullOrWhiteSpace(newConfig.RabbitMqConnectionString))
        {
            return services;
        }

        var rabbitMqUserName = ExtractRabbitMqUserName(newConfig.RabbitMqConnectionString);

        services.AddMassTransit(x =>
        {
            var temp = Guid.NewGuid().ToString("N");
            var prefix = $"{rabbitMqUserName}__bff__{temp}__";

            Log.Warning("{Prefix} used for generating exchanges and queues. {DateTime}, {SecurityLog}",
                prefix, DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), 0);

            x.AddInMemoryInboxOutbox();

            x.SetEndpointNameFormatter(new SnakeCaseEndpointNameFormatter(prefix, false));

            x.AddConsumer<UserLoggedOutConsumer, ResilientConsumerDefinition<UserLoggedOutConsumer>>()
                .Endpoint(e => e.Temporary = true);
            x.AddConsumer<UserLoggedOutFaultConsumer, ResilientConsumerDefinition<UserLoggedOutFaultConsumer>>()
                .Endpoint(e => e.Temporary = true);
            x.AddConsumer<UserForceLoggedOutConsumer, ResilientConsumerDefinition<UserForceLoggedOutConsumer>>()
                .Endpoint(e => e.Temporary = true);
            x.AddConsumer<UserForceLoggedOutFaultConsumer, ResilientConsumerDefinition<UserForceLoggedOutFaultConsumer>>()
                .Endpoint(e => e.Temporary = true);

            x.AddConsumer<FaultConsumer, ResilientConsumerDefinition<FaultConsumer>>()
                .Endpoint(e => e.Temporary = true);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(newConfig.RabbitMqConnectionString));

                cfg.Message<Fault>(f =>
                {
                    f.SetEntityName($"{prefix}_fault");
                });

                cfg.Message<Fault<IUserLoggedOut>>(f =>
                {
                    f.SetEntityName($"{prefix}_user_logged_out_fault");
                });

                cfg.Publish<Fault>(p =>
                {
                    p.AutoDelete = true;
                    p.Exclude = true;
                });

                cfg.Publish<Fault<IUserLoggedOut>>(p =>
                {
                    p.AutoDelete = true;
                    p.Exclude = true;
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        if (!newConfig.StartBusAutomatically)
        {
            services.RemoveMassTransitHostedService();
        }

        return services;
    }

    private static string ExtractRabbitMqUserName(string connectionString)
    {
        var userName = connectionString.Split('@')[0]?.Split("//")?.LastOrDefault()?.Split(':')[0];

        if (string.IsNullOrWhiteSpace(userName))
        {
            return "defautl";
        }

        return userName;
    }
}