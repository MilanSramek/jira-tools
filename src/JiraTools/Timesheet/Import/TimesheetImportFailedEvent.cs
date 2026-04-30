using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record TimesheetImportFailedEvent(Exception Error) : INotification;