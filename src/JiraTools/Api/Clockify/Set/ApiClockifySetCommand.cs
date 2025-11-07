using System.CommandLine;
using System.Text.Json.Nodes;

using Clockify;

namespace JiraTools.Api.Clockify.Set;

internal sealed class ApiClockifySetCommand : Command
{
    public ApiClockifySetCommand() : base("set", "Set Clockify connection options")
    {
        var apiTokenOption = new Option<string>("--api-key", "-k")
        {
            Description = "User API token",
        };
        Options.Add(apiTokenOption);

        SetAction((context, cancellationToken) =>
        {
            var apiToken = context.GetValue(apiTokenOption);
            return ExecuteAsync(apiToken, cancellationToken);
        });
    }

    private static async Task ExecuteAsync(string? apiToken, CancellationToken cancellationToken)
    {
        if (apiToken is { })
        {
            await SaveSecretsAsync(apiToken, cancellationToken);
        }
    }

    private static async Task SaveSecretsAsync(string apiKey, CancellationToken cancellationToken)
    {
        JsonObject settingsObject = await SecretsFile.ReadAsync(cancellationToken) ?? new();

        var clockifyObject = settingsObject[ClockifyOptions.SectionName];
        if (clockifyObject is null)
        {
            clockifyObject = new JsonObject();
            settingsObject[ClockifyOptions.SectionName] = clockifyObject;
        }

        clockifyObject[nameof(ClockifyOptions.ApiKey)] = apiKey;
        await SecretsFile.WriteAsync(settingsObject, cancellationToken);
    }
}
