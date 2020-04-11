using System;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        /// <param name="typeToConvert">Type to convert</param>
        /// <param name="options">JSON serializer options</param>
        /// <returns>Language</returns>
        public override ELanguage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ELanguage ret = ELanguage.English;
            if (reader.TokenType == JsonTokenType.String)
            {
                switch (reader.GetString())
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
            }
            return ret;
        }

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Value</param>
        /// <param name="options">JSON serializer options</param>
        public override void Write(Utf8JsonWriter writer, ELanguage value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
