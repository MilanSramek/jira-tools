using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifySummaryReportSettings
{
    [JsonPropertyName("group")]
    public required string Group { get; init; }

    [JsonPropertyName("subgroup")]
    public required string Subgroup { get; init; }
}