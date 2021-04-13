using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ JSON converters namespace
/// </summary>
namespace ScribblersSharp.JSONConverters
{
    /// <summary>
    /// A class used for convert player state to JSON and vice versa
    /// </summary>
    internal class PlayerStateJSONConverter : JsonConverter<EPlayerState>
    {
        /// <summary>
        /// Reads JSON
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
            if (reader.Value is string value)
            {
                if (!Enum.TryParse(Naming.UpperFirstCharacter(value), out ret))
                {
                    ret = EPlayerState.Invalid;
                }
            }
            return ret;
        }

        /// <summary>
        /// Writes JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Player state value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, EPlayerState value, JsonSerializer serializer) => writer.WriteValue(Naming.LowerFirstCharacter(value.ToString()));
    }
}
