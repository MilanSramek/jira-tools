namespace JiraTools.Extensions;

internal static class TimeProviderExtensions
{

    public static (DateOnly StartOfDay, DateOnly EndOfDay) GetCurrentDayDateRange(
        this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().Date);
        return (today, today);
    }

    public static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetCurrentWeekDateRange(
        this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().Date);

        var dayOfWeek = (int)today.DayOfWeek;
        if (dayOfWeek < 0)
        {
            dayOfWeek = 7;
        }

        return (today.AddDays(1 - dayOfWeek), today.AddDays(7 - dayOfWeek));
    }

    public static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetCurrentMonthDateRange(
        this TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().Date);

        var year = today.Year;
        var month = today.Month;
        var lastDay = DateTime.DaysInMonth(year, month);

        return (new DateOnly(year, month, 1), new DateOnly(year, month, lastDay));
    }
}