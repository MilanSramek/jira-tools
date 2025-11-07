using JiraTools.Api.Jira.Set;

using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Api.Jira;

internal static class Registrations
{
    public static IServiceCollection AddJiraCommands(this IServiceCollection services)
    {
        services.AddSingleton<ApiJiraCommand>();
        services.AddApiJiraSetCommands();

        return services;
    }
}
