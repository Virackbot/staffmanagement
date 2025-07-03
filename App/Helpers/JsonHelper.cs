using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Helpers;

public static class JsonHelper
{
    public static string SerializeObject(object? obj, JsonSerializerOptions? option = null)
    {
        if (obj is null)
        {
            return string.Empty;
        }
        return JsonSerializer.Serialize(obj,option); 
    }
    public static string SerializeObjectWithCamelCase<T>(T obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(obj, options);
    }

    public static bool CanDeserialize<T>(string json)
    {
        try
        {
            JsonSerializer.Deserialize<T>(json);
            //JsonConvert.DeserializeObject<T>(json);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static T? DeserializeObject<T>(string? json)
    {
        try
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception)
        {
            return default;
        }
    }

    public static T? NewtonDeserializeObject<T>(string? json)
    {
        try
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception)
        {
            return default;
        }
    }
}

    public class CustomDecimalNullJsonConvert : JsonConverter<decimal?>

{
    public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return 0;
        }
        return reader.TryGetDecimal(out decimal val) ? val : 0;
    }

    public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
    {
        if (!value.HasValue)
        {
            writer.WriteNumberValue(0);
        }
        else
        {
            writer.WriteNumberValue(Convert.ToDecimal(value));
        }
    }
}