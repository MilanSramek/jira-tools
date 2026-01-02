using System.CommandLine;

using JiraTools.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace JiraTools.Timesheet.Import;

internal sealed class ImportTimesheetCommand : Command
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeProvider _timeProvider;

    public ImportTimesheetCommand(
        IServiceScopeFactory scopeFactory,
        TimeProvider timeProvider)
        : base("import", "Import timesheet data to Jira")
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

        Setup();
    }

    private void Setup()
    {
        Option<TimesheetImportSource> timesheetSourceOption = new("--source", "-s")
        {
            Description = "Source of timesheet to import",
            Required = true,
        };
        timesheetSourceOption.AcceptOnlyFromAmong(typeof(TimesheetImportSource).GetEnumNames());
        Options.Add(timesheetSourceOption);

        Option<TimesheetImportTimePeriod> importPeriodOption = new("--period", "-p")
        {
            Description = "Period to import",
            Required = true,
        };
        importPeriodOption.AcceptOnlyFromAmong(typeof(TimesheetImportTimePeriod).GetEnumNames());
        Options.Add(importPeriodOption);

        Option<DateOnly> importAnchorDate = new("--anchor", "-a")
        {
            Description = "Anchor date for the period to import (default: current date; format: YYYY-MM-DD)",
            Required = false,
            DefaultValueFactory = _ => DateOnly.FromDateTime(_timeProvider.GetLocalNow().Date)
        };
        Options.Add(importAnchorDate);

        TreatUnmatchedTokensAsErrors = true;

        SetAction((context, cancellationToken) =>
        {
            var timesheetSource = context.GetRequiredValue(timesheetSourceOption);
            var importPeriod = context.GetRequiredValue(importPeriodOption); 
            var anchorDate = context.GetRequiredValue(importAnchorDate);  

            return ExecuteAsync(timesheetSource, importPeriod, anchorDate, cancellationToken);
        });
    }

    private async Task ExecuteAsync(
        TimesheetImportSource timesheetSource,
        TimesheetImportTimePeriod importTimePeriod,
        DateOnly anchorDate,
        CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var serviceProvider = scope.ServiceProvider;

        var timesheetImporter = timesheetSource switch
        {
            TimesheetImportSource.Clockify => serviceProvider.GetRequiredService<ClockifyTimesheetImporter>(),
            _ => throw new NotSupportedException($"Timesheet source '{timesheetSource}' is not supported.")
        };

        var (importFrom, importTo) = importTimePeriod switch
        {
            TimesheetImportTimePeriod.Day => anchorDate.GetSurroundingDayDateRange(),
            TimesheetImportTimePeriod.Week => anchorDate.GetSurroundingWeekDateRange(),
            TimesheetImportTimePeriod.Month => anchorDate.GetSurroundingMonthDateRange(),
            _ => throw new NotSupportedException("Unsupported period")
        };

        await timesheetImporter.ExecuteAsync(
            new TimesheetImportSettings(
                From: importFrom,
                To: importTo),
            cancellationToken);
    }
};