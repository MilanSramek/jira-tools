using System.Text.Json.Serialization;

namespace Clockify;

public sealed record UpdateTimeEntryRequest
{
    [JsonPropertyName("start")]
    public DateTime? Start { get; init; }

    [JsonPropertyName("end")]
    public DateTime? End { get; init; }

    [JsonPropertyName("billable")]
    public bool? Billable { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("projectId")]
    public string? ProjectId { get; init; }

    [JsonPropertyName("taskId")]
    public string? TaskId { get; init; }

    [JsonPropertyName("tagIds")]
    public string[]? TagIds { get; init; }

    [JsonPropertyName("customFields")]
    public ClockifyCustomFieldValue[]? CustomFields { get; init; }
}