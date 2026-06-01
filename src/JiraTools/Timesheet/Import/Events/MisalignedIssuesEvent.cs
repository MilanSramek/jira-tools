using Jira;
using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record MisalignedIssuesEvent
(
    IEnumerable<JiraTimesheetEntry> MisalignedJiraIssues,
    IEnumerable<(JiraIssueKey Key, CreateJiraWorklogRequest Request)> NotCreatedEntries
) :
    INotification;