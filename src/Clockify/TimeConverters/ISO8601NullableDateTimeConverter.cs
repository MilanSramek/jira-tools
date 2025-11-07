using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Clockify.TimeConverters;

internal sealed class ISO8601NullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return DateTime.TryParse(value, null, DateTimeStyles.RoundtripKind, out DateTime result)
            ? result
            : throw new JsonException($"Unable to parse '{value}' as DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            var utcValue = value.Value.Kind == DateTimeKind.Utc
                ? value.Value
                : value.Value.ToUniversalTime();
            writer.WriteStringValue(utcValue.ToString(ISO8601TimeFormat.DateTimeFormat));
            return;
        }

        writer.WriteNullValue();
    }
}
