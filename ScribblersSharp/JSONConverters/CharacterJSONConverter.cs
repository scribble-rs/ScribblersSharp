using Newtonsoft.Json;
using System;

namespace ScribblersSharp.JSONConverters
{
    internal class CharacterJSONConverter : JsonConverter<char>
    {
        public override char ReadJson(JsonReader reader, Type objectType, char existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            char ret = existingValue;
            if (reader.Value is long value)
            {
                ret = (char)value;
            }
            return ret;
        }

        public override void WriteJson(JsonWriter writer, char value, JsonSerializer serializer)
        {
            writer.WriteValue((long)value);
        }
    }
}
