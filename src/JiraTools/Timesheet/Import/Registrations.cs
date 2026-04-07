using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Timesheet.Import;

internal static class Registrations
{
    public static void AddTimesheetImportCommands(this IServiceCollection services)
    {
        services.AddTransient<ImportTimesheetCommand>();
        services.AddTransient<TimesheetImporter>();

        services.AddEventHandlers();
    }

    private static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddSingleton<TimesheetImporterEventsHandler>();
        
        services.AddTransient<INotificationHandler<StartingTimesheetImportEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
        services.AddTransient<INotificationHandler<TimesheetImportFinishedEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
        services.AddTransient<INotificationHandler<MisalignedIssuesEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
    }
}