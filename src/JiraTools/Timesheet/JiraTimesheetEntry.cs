using Jira;

namespace JiraTools.Timesheet;

internal sealed record JiraTimesheetEntry
(
    JiraWorklog Worklog,
    JiraIssue Issue
);