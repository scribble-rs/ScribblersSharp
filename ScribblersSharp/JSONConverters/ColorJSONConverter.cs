using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

/// <summary>
/// Scribble.rs ♯ JSON converters namespace
/// </summary>
namespace ScribblersSharp.JSONConverters
{
    /// <summary>
    /// Color JSON converter class
    /// </summary>
    internal class ColorJSONConverter : JsonConverter<Color>
    {
        /// <summary>
        /// Color regular expression
        /// </summary>
        public static readonly Regex colorRegex = new Regex(@"#([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})");

        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Color</returns>
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Color ret = existingValue;
            if (reader.TokenType == JsonToken.String)
            {
                Match match = colorRegex.Match(reader.ReadAsString());
                if (match.Success)
                {
                    if (match.Groups.Count == 4)
                    {
                        ret = Color.FromArgb(int.Parse(match.Groups[1].Value, NumberStyles.HexNumber), int.Parse(match.Groups[2].Value, NumberStyles.HexNumber), int.Parse(match.Groups[3].Value, NumberStyles.HexNumber));
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Color value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteValue(value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2"));
        }
    }
}
