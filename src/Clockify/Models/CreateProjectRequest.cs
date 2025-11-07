using System.Text.Json.Serialization;

namespace Clockify;

public sealed record CreateProjectRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("clientId")]
    public string? ClientId { get; init; }

    [JsonPropertyName("isPublic")]
    public bool IsPublic { get; init; }

    [JsonPropertyName("estimate")]
    public ClockifyProjectEstimate? Estimate { get; init; }

    [JsonPropertyName("color")]
    public string Color { get; init; } = "#f44336";

    [JsonPropertyName("billable")]
    public bool Billable { get; init; } = true;

    [JsonPropertyName("note")]
    public string? Note { get; init; }

    [JsonPropertyName("budgetEstimate")]
    public ClockifyBudgetEstimate? BudgetEstimate { get; init; }

    [JsonPropertyName("timeEstimate")]
    public ClockifyTimeEstimate? TimeEstimate { get; init; }
}