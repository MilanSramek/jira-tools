using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Timesheet.Import;

internal static class Registrations
{
    public static void AddTimesheetImportCommands(this IServiceCollection services)
    {
        services.AddTransient<ImportTimesheetCommand>();
        services.AddTransient<TimesheetImporter>();
        services.AddOptions<TimesheetImporterOptions>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                options.JiraMaxRequestParallelism = configuration.GetValue<int?>("Jira:MaxRequestParallelism");
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

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
        services.AddTransient<INotificationHandler<TimesheetImportFailedEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
        services.AddTransient<INotificationHandler<JiraTimesheetAcquiredEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
        services.AddTransient<INotificationHandler<ClockifyTimesheetAcquiredEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
        services.AddTransient<INotificationHandler<JiraTimesheetAcquiringFailedEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
        services.AddTransient<INotificationHandler<ClockifyTimesheetAcquiringFailedEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
        services.AddTransient<INotificationHandler<JiraTimesheetPublishingFailedEvent>>(
            _ => _.GetRequiredService<TimesheetImporterEventsHandler>());
    }
}