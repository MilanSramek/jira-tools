using System.Text.Json;
using System.Text.Json.Nodes;

namespace JiraTools;

internal static class SecretsFile
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public static readonly string DirectoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "JiraTools");

    public static readonly string FilePath = Path.Combine(DirectoryPath, "secrets.json");

    public static async Task<JsonObject?> ReadAsync(CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(DirectoryPath);

        return File.Exists(FilePath)
            ? JsonNode.Parse(await File.ReadAllTextAsync(FilePath, cancellationToken))!.AsObject()
            : null;
    }

    public static Task WriteAsync(JsonObject content, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(DirectoryPath);

        return File.WriteAllTextAsync(
            FilePath,
            content.ToJsonString(s_jsonSerializerOptions),
            cancellationToken);
    }
}