using System.CommandLine;

using Clockify;

using Jira;

using JiraTools;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// args = ["timesheet", "import", "--source", "Clockify", "--period", "Month"];
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile(SettingsFile.FilePath, optional: true, reloadOnChange: false);
        config.AddJsonFile(SecretsFile.FilePath, optional: true, reloadOnChange: false);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddClockify();
        services.AddJira();
        services.AddCommands();

        services.AddOptions<ClockifyOptions>()
            .Bind(context.Configuration.GetSection(ClockifyOptions.SectionName));
        services.AddOptions<JiraOptions>()
            .Bind(context.Configuration.GetSection(JiraOptions.SectionName));
    })
    .Build();

JiraToolsRootCommand rootCommand = host.Services.GetRequiredService<JiraToolsRootCommand>();
ParseResult parseResult = rootCommand.Parse(args);

IHostApplicationLifetime lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
await parseResult.InvokeAsync(cancellationToken: lifetime.ApplicationStopping);