using Jira;
using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record MisalignedIssuesEvent
(
    IEnumerable<JiraTimesheetEntry> NotAligningJiraEntries,
    IEnumerable<(JiraIssueKey Key, CreateJiraWorklogRequest Request)> NotCreatedEntries
) :
    INotification;