using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyProjectEstimate
{
    [JsonPropertyName("estimate")]
    public required string Estimate { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("resetOption")]
    public string? ResetOption { get; init; }

    [JsonPropertyName("active")]
    public bool Active { get; init; }
}