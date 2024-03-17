using Duende.Bff;
using Duende.Bff.Yarp;
using Yarp.ReverseProxy.Configuration;

namespace Charisma.AuthenticationManager;

public static class ReverseProxyConfigurator
{
    public static IReverseProxyBuilder LoadFromSimpleConfiguration(this IReverseProxyBuilder builder, IConfiguration configuration)
    {
        var reverseProxyConfig = configuration.GetSection("ReverseProxy").Get<IReadOnlyDictionary<string, string>>();
        var routes = reverseProxyConfig?.Keys.Select(key => new RouteConfig
        {
            RouteId = key,
            ClusterId = key,
            Match = new()
            {
                Path = $"/{key}/" + "{**catch-all}"
            },
            Transforms =
            [
                new Dictionary<string, string>(){{ "PathRemovePrefix", $"/{key}" },},
            ],
        }.WithAccessToken(TokenType.User).WithAntiforgeryCheck()).ToList() ?? [];

        var clusters = reverseProxyConfig?.Select(item => new ClusterConfig
        {
            ClusterId = item.Key,

            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            {
                { item.Key, new() { Address = item.Value, } },
            }
        }).ToList() ?? [];

        builder.LoadFromMemory(routes, clusters);

        return builder;
    }
}
