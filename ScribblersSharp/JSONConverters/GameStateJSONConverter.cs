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
    internal class GameStateJSONConverter : JsonConverter<EGameState>
    {
        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Game state</returns>
        public override EGameState ReadJson(JsonReader reader, Type objectType, EGameState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            EGameState ret = existingValue;
            if (reader.Value is string value)
            {
                if (!Enum.TryParse(Naming.UpperFirstCharacter(value), out ret))
                {
                    ret = EGameState.Unknown;
                }
            }
            return ret;
        }

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Game state value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, EGameState value, JsonSerializer serializer) => writer.WriteValue(Naming.LowerFirstCharacter(value.ToString()));
    }
}
