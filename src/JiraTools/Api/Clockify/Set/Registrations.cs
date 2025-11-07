using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Api.Clockify.Set;

internal static class Registrations
{
    public static IServiceCollection AddClockifySetCommands(this IServiceCollection services)
    {
        services.AddSingleton<ApiClockifySetCommand>();

        return services;
    }
}
