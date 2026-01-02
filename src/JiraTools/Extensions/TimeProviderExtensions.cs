namespace JiraTools.Extensions;

internal static class TimeProviderExtensions
{

    public static (DateOnly StartOfDay, DateOnly EndOfDay) GetCurrentDayDateRange(
        this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().Date);
        return today.GetSurroundingDayDateRange();
    }

    public static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetCurrentWeekDateRange(
        this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().Date);
        return today.GetSurroundingWeekDateRange();
    }

    public static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetCurrentMonthDateRange(
        this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().Date);
        return today.GetSurroundingMonthDateRange();
    }
}