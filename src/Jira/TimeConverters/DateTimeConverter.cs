using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jira.TimeConverters;

internal sealed class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException($"Unable to parse empty string as DateTime.");
        }

        return DateTime.TryParse(value, null, DateTimeStyles.RoundtripKind, out DateTime result)
            ? result
            : throw new JsonException($"Unable to parse '{value}' as DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var utcValue = value.Kind == DateTimeKind.Utc
            ? value
            : value.ToUniversalTime();
        var result = utcValue.ToString(TimeFormats.ISO8601WithMillisecondsAndSuffix);
        writer.WriteStringValue(result);
    }
}
