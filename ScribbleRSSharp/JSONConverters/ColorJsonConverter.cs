using System;
using System.Drawing;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// scribble.rs # JSON converters namespace
/// </summary>
namespace ScribbleRSSharp.JSONConverters
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
        /// <param name="typeToConvert">Type to convert</param>
        /// <param name="options">JSON serializer options</param>
        /// <returns></returns>
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Color ret = Color.Black;
            if (reader.TokenType == JsonTokenType.String)
            {
                Match match = colorRegex.Match(reader.GetString());
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
        /// <param name="value">Value</param>
        /// <param name="options">JSON serializer options</param>
        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2"));
        }
    }
}
