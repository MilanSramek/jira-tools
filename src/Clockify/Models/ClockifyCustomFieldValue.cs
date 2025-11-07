using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyCustomFieldValue
{
    [JsonPropertyName("customFieldId")]
    public required string CustomFieldId { get; init; }

    [JsonPropertyName("value")]
    public string? Value { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }
}