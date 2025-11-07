using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyBudgetEstimate
{
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("resetOption")]
    public string? ResetOption { get; init; }

    [JsonPropertyName("active")]
    public bool Active { get; init; }
}