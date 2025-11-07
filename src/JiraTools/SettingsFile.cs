using System.Text.Json;
using System.Text.Json.Nodes;

namespace JiraTools;

internal static class SettingsFile
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public static readonly string FilePath = "appsettings.json";

    public static async Task<JsonObject?> ReadAsync(CancellationToken cancellationToken)
    {
        return File.Exists(FilePath)
            ? JsonNode.Parse(await File.ReadAllTextAsync(FilePath, cancellationToken))!.AsObject()
            : null;
    }

    public static Task WriteAsync(JsonObject content, CancellationToken cancellationToken)
    {
        return File.WriteAllTextAsync(
            FilePath,
            content.ToJsonString(s_jsonSerializerOptions),
            cancellationToken);
    }
}