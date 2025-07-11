using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StaffManagement.API.Helpers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (DateOnly.TryParseExact(stringValue, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    return date;
            }
            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to DateOnly.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
