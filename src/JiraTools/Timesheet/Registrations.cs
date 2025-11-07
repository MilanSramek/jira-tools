using JiraTools.Api;
using JiraTools.Timesheet.Import;

using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Timesheet;

internal static class Registrations
{
    public static IServiceCollection AddImportCommands(this IServiceCollection services)
    {
        services.AddTransient<TimesheetCommand>();
        services.AddTimesheetImportCommands();
        services.AddApiCommands();

        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ClockifyTimesheetProvider>();
        services.AddTransient<JiraTimesheetProvider>();

        return services;
    }
}