using JiraTools.Timesheet;

using MediatR;

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

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
        services.AddSingleton<IPublisher>(_ => _.GetRequiredService<IMediator>());
        return services;
    }
}