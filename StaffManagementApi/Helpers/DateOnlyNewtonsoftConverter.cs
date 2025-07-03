using Newtonsoft.Json;
using System.Globalization;

namespace StaffManagement.API.Helpers
{
    public class DateOnlyNewtonsoftConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var stringValue = reader.Value?.ToString();
                if (DateOnly.TryParseExact(stringValue, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    return date;
            }
            throw new JsonException($"Unable to convert \"{reader.Value}\" to DateOnly.");
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
