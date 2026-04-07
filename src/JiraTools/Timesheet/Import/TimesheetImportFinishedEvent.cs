using Jira;
using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record TimesheetImportFinishedEvent
(
    IEnumerable<JiraWorklog> ImportedWorklogs
) 
    : INotification;