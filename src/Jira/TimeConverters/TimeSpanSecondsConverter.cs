using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jira.TimeConverters;

/// <summary>
/// Converts TimeSpan to/from seconds (integer) when TimeSpanAsSecondsAttribute is present,
/// otherwise uses default .NET JSON serialization.
/// </summary>
internal sealed class TimeSpanSecondsConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return TimeSpan.Zero;
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            var seconds = reader.GetInt32();
            return TimeSpan.FromSeconds(seconds);
        }

        throw new JsonException($"Cannot convert {reader.TokenType} to TimeSpan");
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        var totalSeconds = (int)value.TotalSeconds;
        writer.WriteNumberValue(totalSeconds);
    }
}
