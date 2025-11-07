using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyTimeInterval
{
    [JsonPropertyName("start")]
    public required DateTime Start { get; init; }

    [JsonPropertyName("end")]
    public DateTime? End { get; init; }

    [JsonPropertyName("duration")]
    public string? Duration { get; init; }
}