using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdventureLandSharp.Core.Util;

public static class JsonOpts {
    public static JsonSerializerOptions Default { get; } = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { 
            new JsonStringEnumConverter(),
            new JsonConverterVector2()
        }
    };

    public static JsonSerializerOptions Condensed { get; } = new(Default) {
        WriteIndented = false,
        Converters = { new JsonConverterBool() }
    };
}

public class JsonConverterBool : JsonConverter<bool> {
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
        reader.TokenType == JsonTokenType.Number ? reader.GetInt32() == 1 : reader.GetBoolean();
    
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteNumberValue(value ? 1 : 0);
}

public class JsonConverterVector2 : JsonConverter<Vector2> {
    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        reader.Read();
        float x = reader.GetSingle();

        reader.Read();
        float y = reader.GetSingle();

        reader.Read();
        return new(x, y);
    }

    public override void Write(Utf8JsonWriter writer, Vector2 vector, JsonSerializerOptions options) {
        writer.WriteStartArray();
        writer.WriteNumberValue(vector.X);
        writer.WriteNumberValue(vector.Y);
        writer.WriteEndArray();
    }
}