namespace JiraTools.Timesheet;

internal sealed record JiraTimesheetProviderOptions
{
    public int? MaxRequestParallelism { get; init; }
}