using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyTask
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("projectId")]
    public required string ProjectId { get; init; }

    [JsonPropertyName("assigneeIds")]
    public string[]? AssigneeIds { get; init; }

    [JsonPropertyName("assigneeId")]
    public string? AssigneeId { get; init; }

    [JsonPropertyName("userGroupIds")]
    public string[]? UserGroupIds { get; init; }

    [JsonPropertyName("estimate")]
    public string? Estimate { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("duration")]
    public string? Duration { get; init; }

    [JsonPropertyName("billable")]
    public bool Billable { get; init; }

    [JsonPropertyName("hourlyRate")]
    public ClockifyHourlyRate? HourlyRate { get; init; }

    [JsonPropertyName("costRate")]
    public ClockifyHourlyRate? CostRate { get; init; }

    [JsonPropertyName("budgetEstimate")]
    public int? BudgetEstimate { get; init; }
}