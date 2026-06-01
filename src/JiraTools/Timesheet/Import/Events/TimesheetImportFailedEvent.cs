using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record TimesheetImportFailedEvent(Exception Error) : INotification;