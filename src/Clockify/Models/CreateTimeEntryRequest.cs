using System.Text.Json.Serialization;

namespace Clockify;

public sealed record CreateTimeEntryRequest
{
    [JsonPropertyName("start")]
    public required DateTime Start { get; init; }

    [JsonPropertyName("end")]
    public DateTime? End { get; init; }

    [JsonPropertyName("billable")]
    public bool? Billable { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("projectId")]
    public required string ProjectId { get; init; }

    [JsonPropertyName("taskId")]
    public string? TaskId { get; init; }

    [JsonPropertyName("tagIds")]
    public string[]? TagIds { get; init; }

    [JsonPropertyName("customFields")]
    public ClockifyCustomFieldValue[]? CustomFields { get; init; }
}