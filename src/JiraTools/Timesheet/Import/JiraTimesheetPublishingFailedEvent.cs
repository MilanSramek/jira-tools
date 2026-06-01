using FluentResults;

using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record JiraTimesheetPublishingFailedEvent(IReadOnlyList<IError> Errors) : INotification;