using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record JiraTimesheetAcquiredEvent
(
    IEnumerable<JiraTimesheetEntry> Timesheet
) 
    : INotification;