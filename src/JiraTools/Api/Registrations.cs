using JiraTools.Api.Clockify;
using JiraTools.Api.Jira;

using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Api;

internal static class Registrations
{
    public static IServiceCollection AddApiCommands(this IServiceCollection services)
    {
        services.AddSingleton<ApiCommand>();
        services.AddJiraCommands();
        services.AddClockifyCommands();

        return services;
    }
}