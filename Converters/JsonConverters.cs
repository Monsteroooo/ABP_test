using System.Text.Json;
using System.Text.Json.Serialization;

namespace ABP_test.Converters;

/// <summary>
/// Serializes/deserializes TimeOnly as "HH:mm" string (e.g. "10:00").
/// </summary>
public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string Format = "HH:mm";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()
            ?? throw new JsonException("Expected a time string in HH:mm format");

        if (!TimeOnly.TryParseExact(value, Format, out var time))
            throw new JsonException($"Invalid time format '{value}'. Expected HH:mm (e.g. \"10:00\")");

        return time;
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}

/// <summary>
/// Serializes/deserializes DateOnly as "yyyy-MM-dd" string (e.g. "2024-09-01").
/// </summary>
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()
            ?? throw new JsonException("Expected a date string in yyyy-MM-dd format");

        if (!DateOnly.TryParseExact(value, Format, out var date))
            throw new JsonException($"Invalid date format '{value}'. Expected yyyy-MM-dd (e.g. \"2024-09-01\")");

        return date;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}
