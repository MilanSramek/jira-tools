using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Timesheet.Import;

internal static class Registrations
{
    public static void AddTimesheetImportCommands(this IServiceCollection services)
    {
        services.AddTransient<ImportTimesheetCommand>();
        services.AddTransient<ClockifyTimesheetImporter>();
    }
}