using System.CommandLine;
using System.Text.Json.Nodes;

using Jira;

namespace JiraTools.Api.Jira.Set;

internal sealed class ApiJiraSetCommand : Command
{
    public ApiJiraSetCommand() : base("set", "Set Jira connection options")
    {
        var baseUrlOption = new Option<string>("--base-url", "-u")
        {
            Description = "Jira API base URL (e.g., https://your-domain.atlassian.net)",
        };
        Options.Add(baseUrlOption);

        var emailOption = new Option<string>("--user-email", "-e")
        {
            Description = "User email address",
        };
        Options.Add(emailOption);

        var apiKeyOption = new Option<string>("--api-key", "-k")
        {
            Description = "User API key",
        };
        Options.Add(apiKeyOption);

        SetAction((context, cancellationToken) =>
        {
            var baseUrl = context.GetValue(baseUrlOption);
            var email = context.GetValue(emailOption);
            var apiKey = context.GetValue(apiKeyOption);

            return ExecuteAsync(baseUrl, email, apiKey, cancellationToken);
        });
    }

    private static async Task ExecuteAsync(
        string? baseUrl,
        string? email,
        string? apiKey, CancellationToken cancellationToken)
    {
        var saveBaseUrlTask = baseUrl is { }
            ? SaveBaseUrlAsync(baseUrl, cancellationToken)
            : Task.CompletedTask;
        var saveSecretsTask = email is { } || apiKey is { }
            ? SaveSecretsAsync(email, apiKey, cancellationToken)
            : Task.CompletedTask;

        await Task.WhenAll(saveBaseUrlTask, saveSecretsTask);
    }

    private static async Task SaveBaseUrlAsync(string baseUrl, CancellationToken cancellationToken)
    {
        var settingsObject = await SettingsFile.ReadAsync(cancellationToken) ?? new();
        var jiraObject = settingsObject[JiraOptions.SectionName];
        if (jiraObject is null)
        {
            jiraObject = new JsonObject();
            settingsObject[JiraOptions.SectionName] = jiraObject;
        }
        jiraObject[nameof(JiraOptions.ApiBaseUrl)] = baseUrl;

        await SettingsFile.WriteAsync(settingsObject, cancellationToken);
    }

    private static async Task SaveSecretsAsync(string? email, string? apiToken, CancellationToken cancellationToken)
    {
        JsonObject settingsObject = await SecretsFile.ReadAsync(cancellationToken) ?? new();

        var jiraObject = settingsObject[JiraOptions.SectionName];
        if (jiraObject is null)
        {
            jiraObject = new JsonObject();
            settingsObject[JiraOptions.SectionName] = jiraObject;
        }

        if (email is { })
        {
            jiraObject[nameof(JiraOptions.Email)] = email;
        }
        if (apiToken is { })
        {
            jiraObject[nameof(JiraOptions.ApiKey)] = apiToken;
        }

        await SecretsFile.WriteAsync(settingsObject, cancellationToken);
    }
}
