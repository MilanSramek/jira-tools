using JiraTools.Api.Clockify.Set;

using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Api.Clockify;

internal static class Registrations
{
    public static IServiceCollection AddClockifyCommands(this IServiceCollection services)
    {
        services.AddSingleton<ApiClockifyCommand>();
        services.AddClockifySetCommands();

        return services;
    }
}
