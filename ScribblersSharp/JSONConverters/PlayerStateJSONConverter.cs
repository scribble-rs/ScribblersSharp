using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ JSON converters namespace
/// </summary>
namespace ScribblersSharp.JSONConverters
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
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Player state</returns>
        public override EPlayerState ReadJson(JsonReader reader, Type objectType, EPlayerState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            EPlayerState ret = existingValue;
            switch (reader.Value.ToString())
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
            return ret;
        }

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Player state value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, EPlayerState value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToLower());
        }
    }
}
