using System.Text.Json;

namespace Charisma.AuthenticationManager.Extensions;

public static class CharismaJson
{
    private static readonly JsonSerializerOptions _options = new()
    {
#if DEBUG
        WriteIndented = true,
#else
        WriteIndented = false,
#endif
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static string Serialize<T>(T input)
    {
        return JsonSerializer.Serialize(input, _options);
    }

    public static T? Deserialize<T>(string input)
    {
        return JsonSerializer.Deserialize<T>(input);
    }
}
