using System;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # JSON converters namespace
/// </summary>
namespace ScribbleRSSharp.JSONConverters
{
    /// <summary>
    /// Player state JSON converter class
    /// </summary>
    internal class PlayerStateJSONConverter : JsonConverter<EPlayerState>
    {
        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="typeToConvert">Type to convert</param>
        /// <param name="options">JSON serializer options</param>
        /// <returns></returns>
        public override EPlayerState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            EPlayerState ret = EPlayerState.Standby;
            if (reader.TokenType == JsonTokenType.String)
            {
                switch (reader.GetString())
                {
                    case "standby":
                        ret = EPlayerState.Standby;
                        break;
                    case "drawing":
                        ret = EPlayerState.Drawing;
                        break;
                    case "guessing":
                        ret = EPlayerState.Guessing;
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
        public override void Write(Utf8JsonWriter writer, EPlayerState value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
