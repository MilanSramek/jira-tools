using System.CommandLine;

using Clockify;

using Jira;

using JiraTools;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

string[] _args_ = ["timesheet", "import", "--source", "Clockify", "--period", "Week"];
// string[] _args_ = ["api", "Jira", "set", "--api-key", "NDZlZTRiZjAtMDkxZC00NTM2LTg0OTktNTcyYTUyNjI2ZmMw"];
IHost host = Host.CreateDefaultBuilder(_args_)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile(SettingsFile.FilePath, optional: true, reloadOnChange: true);
        config.AddJsonFile(SecretsFile.FilePath, optional: true, reloadOnChange: true);
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

var config = host.Services.GetService<IOptions<ClockifyOptions>>();
var jiraConfig = host.Services.GetService<IOptions<JiraOptions>>();

JiraToolsRootCommand rootCommand = host.Services.GetRequiredService<JiraToolsRootCommand>();
ParseResult parseResult = rootCommand.Parse(_args_);

IHostApplicationLifetime lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
await parseResult.InvokeAsync(cancellationToken: lifetime.ApplicationStopping);