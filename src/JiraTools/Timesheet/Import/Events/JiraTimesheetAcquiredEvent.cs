using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record JiraTimesheetAcquiredEvent
(
    IEnumerable<JiraTimesheetEntry> Timesheet
) 
    : INotification;