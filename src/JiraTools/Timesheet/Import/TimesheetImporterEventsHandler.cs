using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed class TimesheetImporterEventsHandler : 
    INotificationHandler<StartingTimesheetImportEvent>,
    INotificationHandler<TimesheetImportFinishedEvent>,
    INotificationHandler<MisalignedIssuesEvent>
{
    public Task Handle(StartingTimesheetImportEvent notification, CancellationToken _)
    {
        var settings = notification.Settings;
        Console.WriteLine(
            $"Starting timesheet import for period [{settings.From:d}, {settings.To:d}]");

        return Task.CompletedTask;
    }

    public Task Handle(MisalignedIssuesEvent notification, CancellationToken _)
    {
        Console.Error.WriteLine("Error: Import cannot proceed because some existing issues do not align.");
        foreach (var worklog in notification.NotAligningJiraEntries)
        {
            Console.Error.WriteLine(
                $"- Issue: {worklog.Issue.Key}, "
                + $"Started: {worklog.Worklog.Started:d}, "
                + $"Time Spent: {worklog.Worklog.TimeSpent}, "
                + $"Comment: {worklog.Worklog.Comment?.GetText() ?? "<no comment>"}");
        }
        Console.Out.WriteLine("Warning: The following worklogs will not be created:");
        foreach (var (Key, Request) in notification.NotCreatedEntries)
        {
            Console.Out.WriteLine(
                $"- Issue: {Key}, "
                + $"Started: {Request.Started:d}, "
                + $"Time Spent: {Request.TimeSpent}, "
                + $"Comment: {Request.Comment?.GetText() ?? "<no comment>"}");   
        }
        
        return Task.CompletedTask;
    }

    public Task Handle(TimesheetImportFinishedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Timesheet import finished. {notification.ImportedWorklogs.Count()} worklogs imported.");
        
        return Task.CompletedTask;
    }
}