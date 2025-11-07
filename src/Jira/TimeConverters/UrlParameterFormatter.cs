using System.Reflection;

using Refit;

namespace Jira.TimeConverters;

internal sealed class UrlParameterFormatter : DefaultUrlParameterFormatter
{
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
    {
        if (value is DateTime dateTime)
        {
            var utcValue = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : dateTime.ToUniversalTime();

            if (attributeProvider.GetCustomAttributes(typeof(UnixTimestampMillisecondsAttribute), false).Length > 0)
            {
                var milliseconds = (long)(utcValue - UnixEpoch).TotalMilliseconds;
                return milliseconds.ToString();
            }

            if (attributeProvider.GetCustomAttributes(typeof(TimeAttribute), false) is [var timeAttribute])
            {
                return utcValue.ToString(((TimeAttribute)timeAttribute).Format);
            }

            return utcValue.ToString();
        }

        return base.Format(value, attributeProvider, type);
    }
}
