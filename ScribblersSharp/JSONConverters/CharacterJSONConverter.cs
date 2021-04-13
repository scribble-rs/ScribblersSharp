using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ JSON converters namespace
/// </summary>
namespace ScribblersSharp.JSONConverters
{
    /// <summary>
    /// A class used for convert characters to JSON and vice versa
    /// </summary>
    internal class CharacterJSONConverter : JsonConverter<char>
    {
        /// <summary>
        /// Reads JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Character</returns>
        public override char ReadJson(JsonReader reader, Type objectType, char existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            char ret = existingValue;
            if (reader.Value is long value)
            {
                ret = (char)value;
            }
            return ret;
        }

        /// <summary>
        /// Writes JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Character value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, char value, JsonSerializer serializer) => writer.WriteValue((long)value);
    }
}
