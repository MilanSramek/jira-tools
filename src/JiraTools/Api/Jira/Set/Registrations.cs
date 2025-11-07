using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Api.Jira.Set;

internal static class Registrations
{
    public static IServiceCollection AddApiJiraSetCommands(this IServiceCollection services)
    {
        services.AddSingleton<ApiJiraSetCommand>();

        return services;
    }
}
