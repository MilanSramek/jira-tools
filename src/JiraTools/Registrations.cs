using JiraTools.Timesheet;

using Microsoft.Extensions.DependencyInjection;

namespace JiraTools;

internal static class Registrations
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddTransient<JiraToolsRootCommand>();
        services.AddImportCommands();

        services.AddServices();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        return services;
    }
}