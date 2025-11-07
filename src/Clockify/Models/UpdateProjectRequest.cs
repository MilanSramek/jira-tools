using System.Text.Json.Serialization;

namespace Clockify;

public sealed record UpdateProjectRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("clientId")]
    public string? ClientId { get; init; }

    [JsonPropertyName("isPublic")]
    public bool? IsPublic { get; init; }

    [JsonPropertyName("estimate")]
    public ClockifyProjectEstimate? Estimate { get; init; }

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("billable")]
    public bool? Billable { get; init; }

    [JsonPropertyName("note")]
    public string? Note { get; init; }

    [JsonPropertyName("archived")]
    public bool? Archived { get; init; }

    [JsonPropertyName("budgetEstimate")]
    public ClockifyBudgetEstimate? BudgetEstimate { get; init; }

    [JsonPropertyName("timeEstimate")]
    public ClockifyTimeEstimate? TimeEstimate { get; init; }
}