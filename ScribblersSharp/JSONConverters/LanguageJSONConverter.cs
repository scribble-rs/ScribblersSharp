using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ JSON converters namespace
/// </summary>
namespace ScribblersSharp.JSONConverters
{
    /// <summary>
    /// Color JSON converter class
    /// </summary>
    internal class LanguageJSONConverter : JsonConverter<ELanguage>
    {
        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Language</returns>
        public override ELanguage ReadJson(JsonReader reader, Type objectType, ELanguage existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            ELanguage ret = existingValue;
            switch (reader.Value.ToString())
            {
                case "english":
                    ret = ELanguage.English;
                    break;
                case "italian":
                    ret = ELanguage.Italian;
                    break;
                case "german":
                    ret = ELanguage.German;
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Language value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, ELanguage value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToLower());
        }
    }
}
