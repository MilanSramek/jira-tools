using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyHourlyRate
{
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }
}