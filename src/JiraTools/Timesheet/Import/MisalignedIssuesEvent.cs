using Jira;
using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record MisalignedIssuesEvent
(
    IEnumerable<JiraTimesheetEntry> MisalignedJiraIssues,
    IEnumerable<(JiraIssueKey Key, CreateJiraWorklogRequest Request)> NotCreatedEntries
) :
    INotification;