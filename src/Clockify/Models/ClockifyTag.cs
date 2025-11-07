using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyTag
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("workspaceId")]
    public required string WorkspaceId { get; init; }

    [JsonPropertyName("archived")]
    public bool Archived { get; init; }
}