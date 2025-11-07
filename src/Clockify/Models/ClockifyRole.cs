using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyRole
{
    [JsonPropertyName("role")]
    public required string Role { get; init; }
}