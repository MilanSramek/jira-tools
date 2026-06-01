using Jira;
using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record TimesheetImportFinishedEvent
(
    IEnumerable<JiraWorklog> ImportedWorklogs
) 
    : INotification;